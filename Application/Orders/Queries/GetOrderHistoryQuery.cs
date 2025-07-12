using Application.Dtos;
using Domain.Orders;
using Infrasctructure.Repositories.Interfaces;
using System.Linq;

namespace Application.Orders.Queries;
public sealed record GetOrderHistoryQuery(int? UserId = null)
    : IRequest<Result<IEnumerable<OrderDto>>>;

internal sealed class GetOrderHistoryQueryHandler(
    IOrderRepository orderRepository
) : IRequestHandler<GetOrderHistoryQuery, Result<IEnumerable<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrderHistoryQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<Order> orders;

        if (request.UserId is null)
        {
            // Получить все заказы
            orders = await _orderRepository.GetAllOrdersAsync();
        }
        else
        {
            // Получить только заказы пользователя
            orders = await _orderRepository.GetUserOrdersAsync(request.UserId.Value);
        }

        if (orders is null || !orders.Any())
        {
            return Result.Failure<IEnumerable<OrderDto>>("Заказы не найдены.");
        }

        var orderDtos = orders.Select(order =>
        {
            var itemsForOrder = order.Items.Select(x => new OrderItemDto(
                x.Id,
                x.OrderId,
                x.ProductId,
                x.Product?.Name ?? "Неизвестный продукт",
                x.Product?.Category?.Name ?? "Неизвестная категория",
                x.Quantity,
                x.Price
            )).ToList();

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

            return new OrderDto(
                order.Id,
                order.UserId,
                userDto,  
                order.CreatedDate,
                order.Status,
                itemsForOrder,
                itemsForOrder.Sum(x => x.Price * x.Quantity)
            );
        });

        return Result.Success(orderDtos);
    }
}