using Domain.Categories;
using Domain.Orders;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrasctructure.Repositories.Classes;

public class OrderItemRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<OrderItem>(dbContext, cache), IOrderItemRepository
{

    public async Task<OrderItem?> GetAsync(int id)
    {
       return await _dbContext.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(oi => oi.Id == id);
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
        var existingOrderItem = await _dbContext.OrderItems.FindAsync(orderItem.Id);
        if (existingOrderItem is not null)
        {
            
            if (existingOrderItem.Quantity != orderItem.Quantity)
            {
                existingOrderItem.Quantity = orderItem.Quantity; 
                _dbContext.OrderItems.Update(existingOrderItem);
            }

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

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(IEnumerable<int> orderIds)
    {
        return await _dbContext.OrderItems
            .Where(oi => orderIds.Contains(oi.OrderId))
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .ToListAsync();
    }

    public async Task<List<OrderItem>> GetByUserIdAsync(int userId)
    {
        return await _dbContext.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .Where(oi => oi.Order.UserId == userId)
            .ToListAsync();
    }

}
