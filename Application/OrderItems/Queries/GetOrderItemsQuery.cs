using Application.Dtos;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;

namespace Application.OrderItems.Queries;
public sealed record GetOrderItemsQuery() : IRequest<Result<List<OrderItemDto>>>;

internal sealed class GetOrderItemsQueryHandler(
    IOrderItemRepository orderItemRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetOrderItemsQuery, Result<List<OrderItemDto>>>
{
    private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result<List<OrderItemDto>>> Handle(GetOrderItemsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser;

        var orderItems = await _orderItemRepository.GetByUserIdAsync(userId.UserId);

        if (orderItems is null || orderItems.Count == 0)
        {
            return Result.Success(new List<OrderItemDto>());
        }

        var response = orderItems.Select(orderItem => new OrderItemDto(
               orderItem.Id,          
               orderItem.OrderId,          
               orderItem.ProductId,
               orderItem.Product?.Name ?? "Неизвестно",
               orderItem.Product?.Category?.Name ?? "Неизвестно",
               orderItem.Quantity,
               orderItem.Price
   )).ToList();

        return Result.Success(response);
    }
}
