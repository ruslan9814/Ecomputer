using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Orders.Queries;

public sealed record GetOrderCommand(
    int UserId,
    int OrderId
) : IRequest<Result<OrderDto>>;

internal sealed class GetOrderCommandHandler(
    IOrderRepository orderRepository,
    IOrderItemRepository itemRepository
    ) : IRequestHandler<GetOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderItemRepository _itemRepository = itemRepository;

    public async Task<Result<OrderDto>> Handle(GetOrderCommand request, 
        CancellationToken cancellationToken)
    {
        var orderIsExists = await _orderRepository.IsExistAsync(request.OrderId);
        var itemOrderIsExists = await _itemRepository.IsExistAsync(request.UserId);
        if (!itemOrderIsExists || !orderIsExists)
        {
            return Result.Failure<OrderDto>("Пользователь не найден.");
        }

        var order = await _orderRepository.GetAsync(request.OrderId);
        var itemOrder = await _itemRepository.GetAsync(request.UserId);

        var response = new OrderDto
        (
            order.Id,
            order.UserId,
            order.CreatedDate,
            order.Status,
            order.Items.Select(x => new OrderItemDto
            (
                itemOrder.ProductId,
                itemOrder.Quantity,
                itemOrder.Price
            )).ToList()
            );

        return Result.Success(response);
    }
}
