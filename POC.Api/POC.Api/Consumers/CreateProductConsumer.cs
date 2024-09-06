using Contracts;
using MassTransit;
using POC.Api.Database;
using POC.Api.Models;

namespace POC.Api.Consumers;

public class CreateProductConsumer: IConsumer<CreateNewProductEvent>
{
    private readonly ProductDbContext _dbContext;

    public CreateProductConsumer(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Consume(ConsumeContext<CreateNewProductEvent> context)
    {
        var product = new Product
        {
            ProductId = context.Message.ProductId,
            ProductName = context.Message.ProductName,
            Category = context.Message.Category,
            SubCategory = context.Message.SubCategory,
        };
        
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }
}