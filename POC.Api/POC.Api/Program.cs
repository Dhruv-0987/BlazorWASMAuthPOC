using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POC.Api;
using POC.Api.Consumers;
using POC.Api.Database;
using POC.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Define security scheme for Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // Note: The Scheme value must be lowercase
        BearerFormat = "JWT"
    });

    // Add requirement to use security scheme for all endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// configure IdentityServer as the token authority
var identityServerSettings = builder.Configuration.GetSection("IdentityServer").Get<IdentityServerSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = identityServerSettings!.Authority;
        options.Audience = identityServerSettings!.Audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidTypes = new[] { "at+jwt" }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors();

builder.Services.AddOptions<RabbitMqOptions>()
    .BindConfiguration(nameof(RabbitMqOptions))
    .ValidateOnStart();

var rabbitMqOptions = builder.Services.BuildServiceProvider()
    .GetRequiredService<IOptions<RabbitMqOptions>>().Value;

builder.Services.AddMassTransit(configurator =>
{
    configurator.SetKebabCaseEndpointNameFormatter();

    configurator.AddConsumer<CreateProductConsumer>();

    configurator.UsingRabbitMq((busFactoryContext, busFactoryConfigurator) =>
    {
        busFactoryConfigurator.Host(new Uri($"amqp://{rabbitMqOptions!.HostName}"), hostingOptions =>
        {
            hostingOptions.Username(rabbitMqOptions.UserName);
            hostingOptions.Password(rabbitMqOptions.Password);
        });

        busFactoryConfigurator.ConfigureEndpoints(busFactoryContext);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.UseCors(builder =>
    builder.AllowAnyOrigin() // adjust the origin accordingly
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapProductApiEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
