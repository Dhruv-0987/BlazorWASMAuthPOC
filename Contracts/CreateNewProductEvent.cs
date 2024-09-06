namespace Contracts;

public class CreateNewProductEvent
{
    public Guid ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string SubCategory { get; set; } = default!;
    public double PricePerUnit { get; set; }
}