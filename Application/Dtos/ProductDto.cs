namespace Application.Dtos;

public sealed record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    bool IsInStock,
    DateTime CreatedDate,
    int Quantity,
    int CategoryId,
    string CategoryName,
    double Rating,
    string ImageUrl  
);
