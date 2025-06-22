using Domain.Products;

namespace Infrasctructure.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> IsProductInStockAsync(int productId);
    Task<int> GetProductStockAsync(int productId);
    Task<IEnumerable<Product>> GetProductsAsync(decimal minPrice, decimal maxPrice);
    Task<Product?> GetProductByIdAsync(int id, bool includeRelated = false);
    Task<IEnumerable<Product>> GetFilteredProductsAsync(
      string? name,
      decimal minPrice,
      decimal maxPrice,
      bool isInStock,
      int? categoryId,
      int page = 1,
      int pageSize = 8);

    public Task<int> GetProductCountAsync(
     string? name,
     decimal minPrice,
     decimal maxPrice,
     bool isInStock,
     int? categoryId); 

    public Task<IEnumerable<Product>> GetAll(bool includeRelated = false);

}
