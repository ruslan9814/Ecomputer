using Domain.Categories;

namespace Application.Endpoints.Products.Responses;

public sealed record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    string Description,
    int Quantity,
    bool IsInStock,
    DateTime CreatedDate,
    Category Category);
