 
namespace Application.Dtos;

public sealed record OrderStatisticsDto
(
    int TotalOrders,
    int UniqueUsers,
    decimal TotalRevenue,
    decimal AverageCheck
);

