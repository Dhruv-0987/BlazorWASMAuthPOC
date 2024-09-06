using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace POC.Api.Database;

public class ProductDbContextDesignTimeFactory: IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
        => new(new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlServer("Server=.,2100;Database=BlazorAppIdentity;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=true;")
            .Options);
}