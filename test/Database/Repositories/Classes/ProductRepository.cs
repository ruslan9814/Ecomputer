using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using test.Database.Repositories.Interfaces;
using test.Models;

namespace test.Database.Repositories.Classes;

public class ProductRepository(ApplicationDbContext dbContext, IDistributedCache cache) :
    BaseRepository<Product>(dbContext, cache), IProductRepository
{

    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IDistributedCache _cache = cache;

    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var prod = await _dbContext.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToListAsync();
        if (prod == null)
        {
            return [];
        }
        return prod;
    }

    public async Task<int> GetProductStockCountAsync(ICartItemRepository _cartItem, int productId)
    {
        var prod = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (prod == null)
        {
            return 0;
        }
        return prod.Quantity;
    }


    public async Task<bool> IsProductInStockAsync(ICartItemRepository _cartItem, int productId)
    {
        var prod = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (prod == null || prod.Quantity <= 0)
        {
            return false;
        }
        return true;
    }
}
