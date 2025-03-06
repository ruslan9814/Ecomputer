using Microsoft.EntityFrameworkCore;
using Domain.Carts;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;

namespace Infrasctructure.Repositories.Classes;

public class CartRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<Cart>(dbContext, cache), ICartRepository
{
    public new async Task<Cart> GetAsync(int cartId) =>
        await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(p => p.Product)
            .FirstAsync(c => c.Id == cartId);

    public async Task<Cart> GetByUserIdAsync(int id) =>
        await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(p => p.Product)
            .FirstAsync(c => c.UserId == id);

    public async Task<bool> IsExistByUserIdAsync(int id) =>
        await _dbContext.Carts.AnyAsync(c => c.UserId == id);

}
