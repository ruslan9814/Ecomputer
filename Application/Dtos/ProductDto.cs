namespace Application.Dtos;

public sealed record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    bool IsInStock,
    DateTime CreatedDate,
    int CategoryId
    //CategoryDto Category
    );
