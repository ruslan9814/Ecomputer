using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Orders.Queries;

public sealed record GetOrderCommand(
    int UserId,
    int OrderId
) : IRequest<Result<OrderDto>>;

internal sealed class GetOrderCommandHandler(
    IOrderRepository orderRepository
) : IRequestHandler<GetOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<OrderDto>> Handle(GetOrderCommand request,
     CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(request.OrderId);

        if (order is null)
        {
            return Result.Failure<OrderDto>("Заказ не найден.");
        }

        var orderItems = order.Items.Select(x => new OrderItemDto(
            x.Id,
            x.OrderId,
            x.ProductId,
            x.Product?.Name ?? "Неизвестно",
            x.Product?.Category?.Name ?? "Неизвестно",
            x.Quantity,
            x.Price
        )).ToList();

        var totalPrice = orderItems.Sum(item => item.Price * item.Quantity);

        
        UserDto? userDto = null;
        if (order.User != null)
        {
            userDto = new UserDto(
                order.User.Id,
                order.User.Name ?? "Unknown",
                order.User.Email ?? "",
                order.User.Address ?? "",
                order.User.ImageUrl
            );
        }

        var response = new OrderDto(
            order.Id,
            order.UserId,
            userDto, 
            order.CreatedDate,
            order.Status,
            orderItems,
            totalPrice
        );

        return Result.Success(response);
    }
}