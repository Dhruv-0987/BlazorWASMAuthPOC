using System.ComponentModel.DataAnnotations;

namespace POC.Api.Models;

public class Product
{
    [Key]
    public Guid ProductId { get; set; } = default!;
    public string? ProductName { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public double PricePerUnit { get; set; }
}