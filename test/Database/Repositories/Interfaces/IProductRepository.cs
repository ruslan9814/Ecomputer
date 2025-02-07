using System.Security.Principal;
using Test.Models;

namespace Test.Database.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> IsProductInStockAsync(int productId); 
    Task<int> GetProductStockCountAsync(int productId);
    Task<IEnumerable<Product>> GetProductsAsync(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<int> GetFilteredProductsCountAsync(int? id,
        string? name,
        decimal? minPrice,
        decimal? maxPrice,
        string? description,
        int? quantity,
        bool? isInStock);
}
