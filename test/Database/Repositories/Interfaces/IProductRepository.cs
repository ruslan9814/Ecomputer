using System.Security.Principal;
using Test.Models;

namespace Test.Database.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> IsProductInStockAsync(int productId); 
    Task<int> GetProductStockCountAsync(int productId);
    Task<IEnumerable<Product>> GetProductsAsync(decimal minPrice, decimal maxPrice);
}
