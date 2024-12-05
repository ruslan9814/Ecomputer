using System.Security.Principal;
using test.Models;

namespace test.Database.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> IsProductInStockAsync(ICartItemRepository _cartItem, int productId);
    Task<int> GetProductStockCountAsync(ICartItemRepository _cartItem, int productId);
    Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
}
