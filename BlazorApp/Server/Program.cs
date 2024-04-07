using Microsoft.AspNetCore.ResponseCompression;
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

// registering custom implementation of the proxy config provider 
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
        options.Authority = "https://localhost:5001";
        
        options.ClientId = "blazor-wasm-client";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("plannr-api");
        options.Scope.Add("recipe-api");

        options.CallbackPath = "/authentication/callback";

        options.MapInboundClaims = false;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
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
