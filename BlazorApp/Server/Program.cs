using Microsoft.AspNetCore.ResponseCompression;
using wasmwithids.Server;
using wasmwithids.Server.Services;
using wasmwithids.Server.Yarp;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// builder.Services.AddProductApiHttpClient();

builder.Services.AddSingleton<HttpContextAccessor>();

builder.Services.AddBff();

// pulling reverse proxy config (routes and cluster) from appsettings
var proxyConfigOptions = builder.Configuration.GetSection("ReverseProxyConfig").Get<ProxyConfigOptions>();

// registering custom In memory implementation of the proxy config provider 
builder.Services.AddSingleton<IProxyConfigProvider>(cp => new InMemoryYarpConfigProvider(proxyConfigOptions));

// this will automatically get the config from the custom InMemoryYarpConfigProvider 
builder.Services.AddReverseProxy()
    .AddTransforms<AccessTokenTransformProvider>(); // AccessTokenTransformProvider to put the bearer token in the header for every req

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-blazor";
        options.Cookie.SameSite = SameSiteMode.Strict;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        var openIdConnectOptions = builder.Configuration.GetSection(nameof(OpenIdConnectOptions)).Get<OpenIdConnectOptions>();
        
        options.Authority = openIdConnectOptions.Authority;
        
        options.ClientId = openIdConnectOptions.ClientId;
        options.ClientSecret = openIdConnectOptions.ClientSecret;
        options.ResponseType = openIdConnectOptions.ResponseType;
        options.ResponseMode = openIdConnectOptions.ResponseMode;

        options.Scope.Clear();
        
        foreach (var scope in openIdConnectOptions.Scope)
        {
            options.Scope.Add(scope);
        }

        options.CallbackPath = openIdConnectOptions.CallbackPath;

        options.MapInboundClaims = openIdConnectOptions.MapInboundClaims;
        options.GetClaimsFromUserInfoEndpoint = openIdConnectOptions.GetClaimsFromUserInfoEndpoint;
        options.SaveTokens = openIdConnectOptions.SaveTokens;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// mapping the product api endpoint we created 
 
// app.MapProductApiEndpoints();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();

app.MapReverseProxy();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
