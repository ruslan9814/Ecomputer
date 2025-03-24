using Application.Dtos;
using Domain.Orders;
using Infrasctructure.Repositories.Interfaces;

namespace Application.OrderItems.Queries;

public sealed record GetByIdOrderItemsQuery(int Id
    ) : IRequest<Result<OrderItemDto>>;

internal sealed class GetByIdOrderItemsQueryHandler(IOrderItemRepository orderItemRepository) : 
    IRequestHandler<GetByIdOrderItemsQuery, Result<OrderItemDto>>
{
    private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;

    public async Task<Result<OrderItemDto>> Handle(GetByIdOrderItemsQuery request, 
        CancellationToken cancellationToken)
    {
        var orderItemIsExists = await _orderItemRepository.IsExistAsync(request.Id);
        if (!orderItemIsExists)
        {
            return Result.Failure<OrderItemDto>("Заказы не найдены.");
        }
        var orderItem = await _orderItemRepository.GetAsync(request.Id);

        var response = new OrderItemDto(orderItem.ProductId, orderItem.Quantity, orderItem.Price);

        return Result.Success(response);
    }
}
