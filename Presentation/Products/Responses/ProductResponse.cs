

namespace Presentation.Products.Responses;

public sealed record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    string Description,
    int Quantity,
    bool IsInStock,
    DateTime CreatedDate,
    int CategoryId,
    string CategoryName);

