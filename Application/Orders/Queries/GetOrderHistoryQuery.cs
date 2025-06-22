using Application.Dtos;
using Domain.Orders;
using Infrasctructure.Repositories.Interfaces;

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
            orders = await _orderRepository.GetAllOrdersAsync();
        }
        else
        {
            orders = await _orderRepository.GetUserOrdersAsync(request.UserId.Value);
        }

        if (orders is null || !orders.Any())
        {
            return Result.Failure<IEnumerable<OrderDto>>("Заказы не найдены.");
        }

        var orderDtos = orders.Select(order =>
        {
            var itemsForOrder = order.Items.Select(x => new OrderItemDto(
                x.ProductId,
                x.Product?.Name ?? "Неизвестный продукт",
                x.Product?.Category?.Name ?? "Неизвестная категория",
                x.Quantity,
                x.Price
            )).ToList();

            return new OrderDto(
                order.Id,
                order.UserId,
                order.CreatedDate,
                order.Status,
                itemsForOrder,
                itemsForOrder.Sum(x => x.Price * x.Quantity)
            );
        });

        return Result.Success(orderDtos);
    }
}

