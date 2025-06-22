using Domain.Carts;

namespace Infrasctructure.Repositories.Interfaces;

public interface ICartItemRepository : IBaseRepository<CartItem>
{
    //Task AddToCartAsync(int cartId, int productId, int quantity);
    //Task RemoveFromCartAsync(int cartId, int porductId);
    Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
    //Task<CartItem> FindItemInCartAsync(int cartId, int productId);
    Task<CartItem?> GetWithProductAndCartAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<CartItem> GetByCartAndProductAsync(int cartId, int productId);
    Task RemoveRangeAsync(IEnumerable<CartItem> cartItems, int userId);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
