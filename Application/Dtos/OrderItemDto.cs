namespace Application.Dtos;
public sealed record OrderItemDto(
    int ProductId,
    int Quantity,
    decimal Price
);
