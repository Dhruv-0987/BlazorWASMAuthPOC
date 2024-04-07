namespace POC.Api2;

public class RecipeDetails
{
    public int Id { get; set; }
    public string RecipeTitle { get; set; }
    public int Servings { get; set; }
    public double Protein { get; set; }
    public double Energy { get; set; }
    public double Carbohydrates { get; set; }
    public double TotalFats { get; set; }
    public string ImageUrl { get; set; }
    public double Cost { get; set; }
    public string Cuisine { get; set; }
    public string Type { get; set; }
    public double AverageRating { get; set; }
    public double HealthRating { get; set; }
    public List<string> Ingredients { get; set; }
    public List<string> Instructions { get; set; }

    public RecipeDetails()
    {
        
    }
}