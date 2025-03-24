using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Users.Queries;

public sealed record GetOrderHistoryQuery(int UserId) : IRequest<Result<IEnumerable<OrderDto>>>;

internal sealed class GetOrderHistoryQueryHandler(
    IOrderRepository orderRepository, IOrderItemRepository orderItemRepository
) : IRequestHandler<GetOrderHistoryQuery, Result<IEnumerable<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
    public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrderHistoryQuery request, 
        CancellationToken cancellationToken)
    {
        var orderIsExists = await _orderRepository.IsExistAsync(request.UserId);
        if (!orderIsExists)
        {
            return Result.Failure<IEnumerable<OrderDto>>("Заказы не найдены.");
        }
        var orderItemsIsExists = await _orderItemRepository.IsExistAsync(request.UserId);
        if (!orderItemsIsExists)
        {
            return Result.Failure<IEnumerable<OrderDto>>("Заказы не найдены.");
        }
        var orders = await _orderRepository.GetByUserIdAsync(request.UserId);
        var orderItems = await _orderItemRepository.GetByOrderIdAsync(orders.Select(o => o.Id));

        var orderDtos = orders.Select(order =>
        {
            var itemsForOrder = orderItems
                .Where(x => x.OrderId == order.Id)
                .Select(x => new OrderItemDto(
                    x.ProductId,
                    x.Quantity,
                    x.Price
                ))
                .ToList();

            return new OrderDto(
                order.Id,
                order.UserId,
                order.CreatedDate,
                order.Status,
                itemsForOrder
            );
        });

        return Result.Success(orderDtos);
    }
}

