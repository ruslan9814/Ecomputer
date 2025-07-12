 
namespace Application.Dtos;

public class OrderStatisticsDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int UniqueUsers { get; set; }
    public decimal AverageCheck { get; set; }
}
