using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Test.Cache;
using Test.Database.Repositories.Interfaces;
using Test.Models;

namespace Test.Database.Repositories.Classes;

public class CartRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<Cart>(dbContext, cache), ICartRepository
{
    public new async Task<Cart> GetAsync(int cartId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(p => p.Product)
            .FirstAsync(c => c.Id == cartId);

        return cart;
    }
}
