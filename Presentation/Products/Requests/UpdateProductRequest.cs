using Microsoft.AspNetCore.Http;

namespace Presentation.Products.Requests;

public sealed record UpdateProductRequest(
    int Id,
    string Name,
    decimal Price,
    string Description,
    int Quantity,
    bool IsInStock,
    int CategoryId,
    IFormFile ImageFile);
