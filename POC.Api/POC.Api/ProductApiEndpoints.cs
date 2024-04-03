using POC.Api.Models;

namespace POC.Api;

public static class ProductApiEndpoints
{
    public static void MapProductApiEndpoints(this WebApplication app)
    {
        app.MapGet("api/product/GetAllProducts", async () =>
        {
            var random = new Random();
            var categories = new[] { "Fruits", "Vegetables", "Dairy", "Bakery", "Meats" };
            var subCategories = new[] { "Organic", "Non-Organic", "Gluten-Free", "Vegan" };
            var productNames = new[] { "Apples", "Bananas", "Carrots", "Milk", "Bread", "Chicken", "Beef", "Oranges", "Lettuce", "Cheese" };

            var products = Enumerable.Range(1, 10).Select(index => new Product
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = productNames[random.Next(productNames.Length)],
                Category = categories[random.Next(categories.Length)],
                SubCategory = subCategories[random.Next(subCategories.Length)],
                PricePerUnit = random.NextDouble() * 10 
            }).ToList();

            return Results.Ok(products);
        }).RequireAuthorization();
    }
}