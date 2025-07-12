using Application.Dtos;
using Domain.Orders;

namespace Application.Dtos;
public record OrderDto(
    int Id,
    int UserId,
    UserDto? User,  
    DateTime CreatedDate,
    OrderStatus Status,
    List<OrderItemDto> Items,
    decimal TotalAmount
);