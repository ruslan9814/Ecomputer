using Microsoft.EntityFrameworkCore;
using Domain.Carts;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;

namespace Infrasctructure.Repositories.Classes;

public class CartItemRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<CartItem>(dbContext, cache), ICartItemRepository
{
    public async Task AddToCartAsync(int cartId, int productId, int quantity)
    {
        var item = await _dbContext.CartItems
            .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

        if (item is not null)// убрать отсюда и прописать в сервисе и в сервисах прописать dto
        {
            item.Quantity += quantity;
            return;
        }

        var newCartItem = new CartItem
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity
        };
        await _dbContext.CartItems.AddAsync(newCartItem);
    }

    public async Task<CartItem> FindItemInCartAsync(int cartId, int productId)
    {
        var item = await _dbContext.CartItems
            .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

        return item is null ? throw new KeyNotFoundException($"CartItem with CartId {cartId} and ProductId {productId} not found.") : item;
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
    {
        return await _dbContext.CartItems
            .Where(c => c.CartId == cartId)
            .Include(c => c.Product)
            .ToListAsync();
    }

    public async Task RemoveFromCartAsync(int cartId, int productId)
    {
        var item = await _dbContext.CartItems
            .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId)
            ?? throw new KeyNotFoundException($"CartItem with CartId {cartId} and ProductId {productId} not found.");
        _dbContext.CartItems.Remove(item);
    }

}
