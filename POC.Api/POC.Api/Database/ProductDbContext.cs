using Microsoft.EntityFrameworkCore;
using POC.Api.Models;

namespace POC.Api.Database;

public class ProductDbContext: DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
}