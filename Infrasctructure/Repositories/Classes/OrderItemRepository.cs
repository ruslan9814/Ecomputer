using Domain.Orders;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrasctructure.Repositories.Classes
{
    public class OrderItemRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
         BaseRepository<OrderItem>(dbContext, cache), IOrderItemRepository
    {

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(IEnumerable<int> orderIds)
        {
            return await _dbContext.OrderItems
                .Where(oi => orderIds.Contains(oi.OrderId))
                .Include(oi => oi.Product) 
                .ToListAsync();
        }

        public async Task<OrderItem> GetByIdAsync(int orderItemId, int productId)
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Product) 
                .FirstAsync(oi => oi.Id == orderItemId && oi.ProductId == productId);
        }

        public async Task AddProductToOrderAsync(OrderItem orderItem)
        {
            await _dbContext.OrderItems.AddAsync(orderItem);
        }

        public async Task UpdateProductToOrderAsync(OrderItem orderItem)
        {
            var orderItems = await _dbContext.OrderItems.FindAsync(orderItem.Id);
            if (orderItems is not null)
            {
                orderItem.Quantity = orderItems.Quantity;
                _dbContext.OrderItems.Update(orderItem);
            }
        }

        public async Task DeleteProductToOrderAsync(int orderItemId)
        {
            var orderItem = await _dbContext.OrderItems.FindAsync(orderItemId);
            if (orderItem is not null)
            {
                _dbContext.OrderItems.Remove(orderItem);
            }
        }

    }
}
