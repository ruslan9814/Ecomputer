using Domain.Carts;

namespace Infrasctructure.Repositories.Interfaces;

public interface ICartItemRepository : IBaseRepository<CartItem>
{
    Task AddToCartAsync(int cartId, int productId, int quantity);
    Task RemoveFromCartAsync(int cartId, int porductId);
    Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
    Task<CartItem> FindItemInCartAsync(int cartId, int productId);
    Task<CartItem> GetWithProductAsync(int cartId);
}
