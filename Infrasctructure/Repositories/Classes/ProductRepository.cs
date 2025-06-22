using Microsoft.EntityFrameworkCore;
using Domain.Products;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;

namespace Infrasctructure.Repositories.Classes;

public class ProductRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<Product>(dbContext, cache), IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsAsync(decimal minPrice, decimal maxPrice)
        => await _dbContext.Products
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .ToListAsync();

    public async Task<int> GetProductStockAsync(int productId)
    {
        var product = await _dbContext.Products
            .FirstAsync(p => p.Id == productId);

        return product.Quantity;
    }

    public async Task<bool> IsProductInStockAsync(int productId)
    {
        var quantity = await _dbContext.Products
             .Where(x => x.Id == productId)
            .Select(x => x.Quantity).FirstAsync();

        return quantity > 0;
    }

    public async Task<Product?> GetProductByIdAsync(int id, bool includeRelated = false)
    {
        var query = _dbContext.Products
            .Where(p => p.Id == id);

        if (includeRelated)
        {
            query = query.Include(p => p.Category);
        }

        return await query.FirstOrDefaultAsync();
    }


    public async Task<IEnumerable<Product>> GetFilteredProductsAsync(
     string? name,
     decimal minPrice,
     decimal maxPrice,
     bool isInStock,
     int? categoryId,  
     int page = 1,
     int pageSize = 8)
    {
        var query = _dbContext.Products
            .Include(p => p.Category)  
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        if (minPrice > 0)
        {
            query = query.Where(p => p.Price >= minPrice);
        }

        if (maxPrice > 0)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }
         
        if (isInStock)
        {
            query = query.Where(p => p.IsInStock == true);
        }
         
        if (categoryId.HasValue && categoryId.Value != 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return products;
    }


    public async Task<int> GetProductCountAsync(
     string? name,
     decimal minPrice,
     decimal maxPrice,
     bool isInStock,
     int? categoryId)  
    {
        var query = _dbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        if (minPrice > 0)
        {
            query = query.Where(p => p.Price >= minPrice);
        }

        if (maxPrice > 0)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }

        if (isInStock)
        {
            query = query.Where(p => p.IsInStock == true);
        }

        if (categoryId.HasValue && categoryId.Value != 0)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        return await query.CountAsync();
    }


    public async Task<IEnumerable<Product>> GetAll(bool includeRelated = false)
    {
        var query = _dbContext.Set<Product>().AsQueryable();

        if (includeRelated)
        {
            query = query.Include(p => p.Category);
        }

        var products = await query.ToListAsync();
        return products;
    }

}
