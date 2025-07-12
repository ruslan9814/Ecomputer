using Microsoft.AspNetCore.Http;

namespace Presentation.Products.Requests;

public class AddProductRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public bool IsInStock { get; set; }

    public int CategoryId { get; set; }

    public IFormFile? ImageFile { get; set; }
}
