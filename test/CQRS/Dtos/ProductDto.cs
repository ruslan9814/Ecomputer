namespace test.CQRS.Dtos;

public sealed record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    bool IsInStock,
    DateTime CreatedDate
    );
