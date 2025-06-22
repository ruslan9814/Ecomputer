using Microsoft.AspNetCore.Http;

namespace Application.Endpoints.Products.Requests;

public sealed record AddProductRequest(
    string Name,
    decimal Price,
    string Description,
    int Quantity,
    bool IsInStock,
    int CategoryId,
    IFormFile ImageFile);
