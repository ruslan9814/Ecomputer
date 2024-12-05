using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using test.Database.Repositories.Interfaces;
using test.Models;

namespace test.Database.Repositories.Classes;

public class CartItemRepository(ApplicationDbContext dbContext, IDistributedCache cache) :
    BaseRepository<CartItem>(dbContext, cache), ICartItemRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IDistributedCache _cache = cache;

    public async Task AddToCartAsync(int cartId, int productId, int quantity)
    {
        var item = await _dbContext.CartItems
        .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);


        if (item != null)
        {
            item.Quantity += quantity;
            await _dbContext.SaveChangesAsync();
            return;
        }

        var newCartItem = new CartItem
        {
            Id = cartId,
            ProductId = productId,
            Quantity = quantity
        };

        await _dbContext.AddAsync(newCartItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CartItem> FindItemInCartAsync(int cartId, int productId)
    {
        var item = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

        return item ?? throw new Exception($"Item with CartId {cartId} and ProductId {productId} not found.");
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
    {
        return await _dbContext.CartItems.Where(c => c.CartId == cartId).ToListAsync();
    }

    public async Task RemoveFromCartAsync(int cartId, int productId)
    {
        var item = await _dbContext.CartItems
       .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

        if (item != null)
        {
            _dbContext.CartItems.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
