namespace test.Endpoints.Products.Models;

public class ProductRequest
{
    public string Name { get; set; } 
    public decimal Price { get; set; } 
    public string? Description { get; set; } 
    public int Quantity { get; set; }
}
