using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace POC.Api2;

public static class RecipeApiEndpoints
{
    public static void MapRecipeApiEndpoints(this WebApplication app)
    {
        app.MapGet("api/recipe/GetAllRecipes", async ([FromServices] RecipeDbContext recipeDbContext) =>
        {
            var recipes = await recipeDbContext.Recipes.ToListAsync();
            return Results.Ok(recipes);
        });
        
        app.MapGet("/api/recipe/GetRecipeById/{id:int}", async (int id, [FromServices] RecipeDbContext recipeDbContext) =>
        {
            var recipe = await recipeDbContext.Recipes.FindAsync(id);
            return recipe != null ? Results.Ok(recipe) : Results.NotFound();
        }).RequireAuthorization();
        
        app.MapPost("/api/recipe/AddRecipe", async ([FromServices] RecipeDbContext recipeDbContext, 
            [FromServices] IPublishEndpoint publishEndpoint,
            [FromBody] Recipe recipe) =>
        {
            var recipeEntity = new Recipe
            {
                RecipeTitle = recipe.RecipeTitle,
                Servings = recipe.Servings,
                Cost = recipe.Cost,
                Cuisine = recipe.Cuisine,
            };
            
            var productId = Guid.NewGuid();
            await publishEndpoint.Publish(new CreateNewProductEvent()
            {
                ProductId = productId,
                ProductName = $"new product - {productId}",
                Category = "new category",
                SubCategory = "new sub category",
            });
                
            recipeDbContext.Recipes.Add(recipeEntity);
            await recipeDbContext.SaveChangesAsync();
            
            return Results.Created($"/api/recipe/GetRecipeById/{recipe.Id}", recipe);
        });
    }
}