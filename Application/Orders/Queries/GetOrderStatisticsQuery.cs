using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;


namespace Application.Orders.Queries;
public sealed record GetOrderStatisticsQuery(string? StartDate = null, string? EndDate = null) 
    : IRequest<Result<OrderStatisticsDto>>;

public class GetOrderStatisticsQueryHandler(IOrderRepository orderRepository) 
    : IRequestHandler<GetOrderStatisticsQuery, Result<OrderStatisticsDto>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<OrderStatisticsDto>> Handle(GetOrderStatisticsQuery request,
        CancellationToken cancellationToken)
    {

        var (totalOrders, totalRevenue, pendingOrders, completedOrders, uniqueUsers, averageCheck, averageOrderValue)
            = await _orderRepository.GetOrderStatisticsAsync(request.StartDate, request.EndDate);

        var response = new OrderStatisticsDto
        {
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            PendingOrders = pendingOrders,
            CompletedOrders = completedOrders,
            UniqueUsers = uniqueUsers,
            AverageCheck = averageCheck,
            AverageOrderValue = averageOrderValue
        };

        return Result.Success(response);

    }
}
