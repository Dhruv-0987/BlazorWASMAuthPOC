using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POC.Api.Database;

namespace POC.Api;

public static class ProductApiEndpoints
{
    public static void MapProductApiEndpoints(this WebApplication app)
    {
        app.MapGet("api/product/GetAllProducts", async ([FromServices] ProductDbContext productDbContext) =>
        {
            var products = await productDbContext.Products.ToListAsync();
            return Results.Ok(products);
        });
    }
}