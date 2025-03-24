using Domain.Orders;

namespace Application.Dtos;

public sealed record OrderDto(
    int OrderId,
    int UserId,
    DateTime CreatedDate,
    OrderStatus Status,
    IEnumerable<OrderItemDto> OrderItems
);

