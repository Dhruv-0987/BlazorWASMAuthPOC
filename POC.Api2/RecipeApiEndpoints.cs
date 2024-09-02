namespace POC.Api2;

public static class RecipeApiEndpoints
{
    private static readonly List<RecipeDetails> Recipes = new List<RecipeDetails>
    {
        new RecipeDetails()
        {
            Id = 1,
            RecipeTitle = "Chicken Curry",
            Servings = 4,
            Protein = 25.0,
            Energy = 500.0,
            Carbohydrates = 45.0,
            TotalFats = 10.0,
            ImageUrl = "https://example.com/images/chicken-curry.jpg",
            Cost = 15.0,
            Cuisine = "Indian",
            Type = "Main Course",
            AverageRating = 4.5,
            HealthRating = 4.0,
            Ingredients = new List<string> { "Chicken", "Curry Powder", "Coconut Milk" },
            Instructions = new List<string> { "Mix ingredients", "Cook for 30 mins" }
        },
        new RecipeDetails()
        {
            Id = 2,
            RecipeTitle = "Vegetable Stir Fry",
            Servings = 3,
            Protein = 10.0,
            Energy = 300.0,
            Carbohydrates = 40.0,
            TotalFats = 5.0,
            ImageUrl = "https://example.com/images/veggie-stir-fry.jpg",
            Cost = 10.0,
            Cuisine = "Chinese",
            Type = "Main Course",
            AverageRating = 4.0,
            HealthRating = 4.5,
            Ingredients = new List<string> { "Broccoli", "Carrot", "Bell Peppers", "Soy Sauce" },
            Instructions = new List<string> { "Chop vegetables", "Stir fry with soy sauce" }
        },
        new RecipeDetails()
        {
            Id = 3,
            RecipeTitle = "Butter Chicken",
            Servings = 4,
            Protein = 25.0,
            Energy = 600.0,
            Carbohydrates = 20.0,
            TotalFats = 30.0,
            ImageUrl = "https://example.com/images/butter-chicken.jpg",
            Cost = 20.0,
            Cuisine = "Indian",
            Type = "Main Course",
            AverageRating = 4.8,
            HealthRating = 3.5,
            Ingredients = new List<string> { "Chicken", "Butter", "Cream", "Tomatoes", "Garam Masala" },
            Instructions = new List<string> 
            { 
                "Marinate chicken with yogurt and spices", 
                "Cook tomatoes with butter and spices", 
                "Add chicken to the sauce and simmer", 
                "Finish with cream and serve with rice or naan"
            }
        },

        new RecipeDetails()
        {
            Id = 4,
            RecipeTitle = "Chicken Biryani",
            Servings = 5,
            Protein = 30.0,
            Energy = 700.0,
            Carbohydrates = 80.0,
            TotalFats = 25.0,
            ImageUrl = "https://example.com/images/chicken-biryani.jpg",
            Cost = 25.0,
            Cuisine = "Indian",
            Type = "Main Course",
            AverageRating = 4.9,
            HealthRating = 4.0,
            Ingredients = new List<string> { "Chicken", "Basmati Rice", "Saffron", "Onions", "Various Spices" },
            Instructions = new List<string> 
            { 
                "Marinate chicken with spices and yogurt", 
                "Layer marinated chicken with partially cooked rice", 
                "Seal the pot and cook over low heat", 
                "Garnish with fried onions and serve"
            }
        },
    };
    
    public static void MapRecipeApiEndpoints(this WebApplication app)
    {
        app.MapGet("api/recipe/GetAllRecipes", () =>
        {
            var recipes = Recipes.Select(detail => new Recipe
            {
                Id = detail.Id,
                RecipeTitle = detail.RecipeTitle,
                Servings = detail.Servings,
                Cost = detail.Cost,
                Cuisine = detail.Cuisine
            }).ToList();

            return Results.Ok(recipes);
        });
        
        app.MapGet("/api/recipe/GetRecipeById/{id:int}", (int id) =>
        {
            var recipe = Recipes.FirstOrDefault(r => r.Id == id);
            return recipe != null ? Results.Ok(recipe) : Results.NotFound();
        }).RequireAuthorization();
    }
}