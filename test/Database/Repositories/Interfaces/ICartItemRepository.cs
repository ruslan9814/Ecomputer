using test.Models;

namespace test.Database.Repositories.Interfaces;

public interface ICartItemRepository : IBaseRepository<CartItem>
{
    Task AddToCartAsync(int cartId, int productId, int quantity);
    Task RemoveFromCartAsync(int cartId, int porductId);
    Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
    Task<CartItem> FindItemInCartAsync(int cartId, int productId);
}
