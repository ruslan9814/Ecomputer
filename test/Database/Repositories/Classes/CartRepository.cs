using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using test.Database.Repositories.Interfaces;
using test.Models;

namespace test.Database.Repositories.Classes;

public class CartRepository(ApplicationDbContext dbContext, IDistributedCache cache) :
    BaseRepository<Cart>(dbContext, cache), ICartRepository
{
    public Task<bool> CartExistsAsync(int cartId)
    {
        throw new NotImplementedException();
    }

    public new async Task<Cart> GetAsync(int cartId) => 
        await _dbContext.Carts
            .Include(c => c.User)
            .Include(c => c.Products)
            .ThenInclude(p => p.Product)
            .FirstAsync(c => c.Id == cartId);
}
