using Domain.Orders;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
 

namespace Infrasctructure.Repositories.Classes;

public class OrderRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<Order>(dbContext, cache), IOrderRepository
{
    private static IQueryable<Order> IncludeOrderItemsAndProduct(IQueryable<Order> query)
    {
        return query.Include(o => o.Items)
                    .ThenInclude(i => i.Product);
    }

    public async Task<Order> Get(int orderId)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .FirstAsync(o => o.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(int statusId)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .Where(o => o.OrderStatusId == statusId)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByDateAsync(DateTime date)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .Where(o => o.CreatedDate == date)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, 
        DateTime endDate)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .Where(o => o.CreatedDate >= startDate && o.CreatedDate <= endDate)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByStatusAndDateAsync(int statusId, DateTime date)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .Where(o => o.OrderStatusId == statusId && o.CreatedDate == date)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByStatusAndDateRangeAsync(int statusId, 
        DateTime startDate, DateTime endDate)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .Where(o => o.OrderStatusId == statusId && o.CreatedDate >= startDate && 
                    o.CreatedDate <= endDate)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByUserIdAndDateAsync(int userId, DateTime date)
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders)
                    .Where(o => o.UserId == userId && o.CreatedDate == date)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await IncludeOrderItemsAndProduct(_dbContext.Orders).ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
    {
        return await _dbContext.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Category)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTopProductsAsync(int topCount = 3)
    {
        return await _dbContext.OrderItems
            .Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .Where(oi => oi.Order.OrderStatusId != null) 
            .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Quantity * x.Price)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(topCount)
            .Cast<object>()  
            .ToListAsync();
    }

    public async Task<object> GetOrderStatisticsAsync()
    {
        var orders = await _dbContext.Orders
            .Include(o => o.Items)
            .ToListAsync();

        var totalOrders = orders.Count;

        var uniqueUsers = orders
            .Select(o => o.UserId)
            .Distinct()
            .Count();

        var totalRevenue = orders
            .SelectMany(o => o.Items)
            .Sum(i => i.Quantity * i.Price);

        var averageCheck = totalOrders > 0 ? totalRevenue / totalOrders : 0;

        return new
        {
            TotalOrders = totalOrders,
            UniqueUsers = uniqueUsers,
            TotalRevenue = totalRevenue,
            AverageCheck = averageCheck
        };
    }




}
