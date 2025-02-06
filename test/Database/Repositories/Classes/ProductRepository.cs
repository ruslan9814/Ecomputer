using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Test.Cache;
using Test.Database.Repositories.Interfaces;
using Test.Models;

namespace Test.Database.Repositories.Classes;

public class ProductRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<Product>(dbContext, cache), IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsAsync(decimal minPrice, decimal maxPrice) => 
        await _dbContext.Products
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .ToListAsync();

    public async Task<int> GetProductStockCountAsync(int productId)
    {
        var product = await _dbContext.Products
            .FirstAsync(p => p.Id == productId);

        return product.Quantity;
    }

    public async Task<bool> IsProductInStockAsync(int productId)
    {
        var quantity = await _dbContext.Products
             .Where(x => x.Id == productId)
            .Select(x => x.Quantity).FirstAsync();

        return quantity > 0;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }
}
