using Microsoft.EntityFrameworkCore;

namespace POC.Api2;

public class RecipeDbContext: DbContext
{
    public RecipeDbContext(DbContextOptions<RecipeDbContext> options): base(options)
    {
        
    }
    
    public DbSet<Recipe> Recipes { get; set; }
}