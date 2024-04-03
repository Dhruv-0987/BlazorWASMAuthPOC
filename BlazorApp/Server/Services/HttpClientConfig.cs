namespace wasmwithids.Server.Services;

public static class HttpClientConfig
{
    public static void AddProductApiHttpClient(this IServiceCollection services)
    {
        services.AddTransient<TokenHandler>();
        // adding httpclient for product endpoints
        services.AddHttpClient("ProductApi", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7281/");
        })
        .AddHttpMessageHandler<TokenHandler>();
    }
}