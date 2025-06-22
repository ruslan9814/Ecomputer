using Domain.Orders;

namespace Infrasctructure.Repositories.Interfaces
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(IEnumerable<int> orderId);
        Task<OrderItem> GetByIdAsync(int orderItemId, int productId);
        Task AddProductToOrderAsync(OrderItem orderItem);
        Task UpdateProductToOrderAsync(OrderItem orderItem);
        Task DeleteProductToOrderAsync(int orderItemId);
        Task<OrderItem?> GetAsync(int id);
        Task<List<OrderItem>> GetByUserIdAsync(int userId);
    }
}
