using Microsoft.EntityFrameworkCore;
using Domain.Carts;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;
using Domain.Users;

namespace Infrasctructure.Repositories.Classes;

public class CartItemRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<CartItem>(dbContext, cache), ICartItemRepository
{

    public async Task<CartItem> FindItemInCartAsync(int cartId, int productId)
    {
        var item = await _dbContext.CartItems
            .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

        return item is null ? throw new
            KeyNotFoundException($"CartItem with CartId {cartId} and ProductId {productId} not found.") : item;
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
    {
        return await _dbContext.CartItems
            .Where(c => c.CartId == cartId)
            .Include(c => c.Product)
            .ToListAsync();
    }

    public async Task<CartItem?> GetWithProductAndCartAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.CartItems
            .Include(ci => ci.Cart)
            .Include(ci => ci.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.Cart.UserId == userId, cancellationToken);
    }

    public async Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId)
    {
        return await _dbContext.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public async Task RemoveRangeAsync(IEnumerable<CartItem> cartItems, int userId)
    {
        var cartItemIds = cartItems.Select(c => c.Id).ToList();

        var itemsToRemove = await _dbContext.CartItems
            .Include(ci => ci.Cart)
            .Where(ci => cartItemIds.Contains(ci.Id) && ci.Cart.UserId == userId)
            .ToListAsync();

        _dbContext.CartItems.RemoveRange(itemsToRemove);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.CartItems.AnyAsync(ci => ci.Id == id, cancellationToken);
    }

}
