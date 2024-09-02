using Authsignal;
using Azure.Identity;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace IdentityServerAspNetIdentity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var apiScopes = builder.Configuration.GetSection("ApiScopes").Get<List<ApiScope>>();
        var clients = builder.Configuration.GetSection("Clients").Get<List<CustomClient>>();
        var identityResources = builder.Configuration.GetSection("IdentityResources").Get<List<IdentityResource>>();
        var apiResources = builder.Configuration.GetSection("ApiResources").Get<List<ApiResource>>();
        
        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(identityResources!)
            .AddInMemoryApiScopes(apiScopes!)
            .AddInMemoryClients(clients!)
            .AddInMemoryApiResources(apiResources!)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<MyProfileService>();
        
        builder.Services.Configure<List<CustomClient>>(builder.Configuration.GetSection("Clients"));
        builder.Services.Configure<List<ApiResource>>(builder.Configuration.GetSection("ApiResources"));
        
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            })
            .AddMicrosoftAccount(options =>
            {
                var msOptions = builder.Configuration.GetSection(nameof(MicrosoftOptions)).Get<MicrosoftOptions>();
                options.AuthorizationEndpoint = MicrosoftAccountDefaults.AuthorizationEndpoint + "?prompt=select_account";

                options.ClientId = msOptions.ClientId != "" ? msOptions.ClientId : "wrong_value";
                options.ClientSecret = msOptions.ClientSecret != "" ? msOptions.ClientSecret : "wrong_secret";
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            });
        
        var corsOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("_AllowOriginFromBlazorWASM",
                policy =>
                {
                    policy
                        .WithOrigins(corsOrigins!)
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        
        builder.Services.AddOptions<AuthSignalOptions>()
            .Bind(builder.Configuration.GetSection(nameof(AuthSignalOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        var authSignalOptions = builder.Services.BuildServiceProvider()
            .GetRequiredService<IOptions<AuthSignalOptions>>().Value;
        
        builder.Services.AddSingleton<IAuthsignalClient>(_ => new AuthsignalClient(authSignalOptions.Secret!, baseAddress: authSignalOptions.BaseAddress!));

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors("_AllowOriginFromBlazorWASM");
        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}