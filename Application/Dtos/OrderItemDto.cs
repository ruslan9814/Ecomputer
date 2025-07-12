namespace Application.Dtos;

public sealed record OrderItemDto(
    int Id,                // <-- добавлено
    int OrderId,           // <-- добавлено (опционально, но очень удобно)
    int ProductId,
    string ProductName,
    string CategoryName,
    int Quantity,
    decimal Price
);