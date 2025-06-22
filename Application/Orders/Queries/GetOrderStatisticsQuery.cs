using Infrasctructure.Repositories.Interfaces;


namespace Application.Orders.Queries;
public sealed record GetOrderStatisticsQuery() : IRequest<Result<object>>;

internal sealed class GetOrderStatisticsQueryHandler(IOrderRepository orderRepository) 
    : IRequestHandler<GetOrderStatisticsQuery, Result<object>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<object>> Handle(GetOrderStatisticsQuery request, CancellationToken cancellationToken)
    {
        var stats = await _orderRepository.GetOrderStatisticsAsync();
        return Result.Success(stats);
    }
}

