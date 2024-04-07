namespace wasmwithids.Shared;

public class Recipe
{
    public int Id { get; set; }
    public string RecipeTitle { get; set; }
    public int Servings { get; set; }
    public double Cost { get; set; }
    public string Cuisine { get; set; }

    public Recipe()
    {
        
    }
}