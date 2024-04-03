using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using wasmwithids.Shared;

namespace wasmwithids.Server.Services;

public static class ProductApiEndpoints
{
    public static void MapProductApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api/bff/products", async (HttpContext httpContext, IHttpClientFactory httpClientFactory) =>
            {
                var client = httpClientFactory.CreateClient("ProductApi");
                var request = new HttpRequestMessage(HttpMethod.Get, "api/product/GetAllProducts");

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return Results.StatusCode((int)response.StatusCode);
                }

                // Assuming the response is a JSON array of products
                var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                if (products == null)
                {
                    return Results.NotFound("No products found.");
                }

                return Results.Ok(products);
            })
            .RequireAuthorization(); 
    }
}