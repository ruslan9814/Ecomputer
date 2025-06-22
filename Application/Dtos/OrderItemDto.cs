namespace Application.Dtos;
public sealed record OrderItemDto(
    int ProductId,
    string ProductName,
    string CategoryName,
    int Quantity,
    decimal Price
);
