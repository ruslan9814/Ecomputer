using Domain.Orders;

namespace Infrasctructure.Repositories.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<Order> Get(int orderId);
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Order>> GetByStatusAsync(int statusId);
    Task<IEnumerable<Order>> GetByDateAsync(DateTime date);
    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Order>> GetByStatusAndDateAsync(int statusId, DateTime date);
    Task<IEnumerable<Order>> GetByStatusAndDateRangeAsync(int statusId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Order>> GetByUserIdAndDateAsync(int userId, DateTime date);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
    Task<IEnumerable<object>> GetTopProductsAsync(int topCount = 3);
    Task<object> GetOrderStatisticsAsync();
}
