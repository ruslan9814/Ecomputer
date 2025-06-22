using Domain.Products;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrasctructure.Repositories.Classes;

public class ProductReviewRepository(ApplicationDbContext applicationDbContext,
    ICacheEntityService cacheEntityService)
    : BaseRepository<ProductReview>(applicationDbContext, cacheEntityService),
    IProductReviewRepository
{
    public async Task<List<ProductReview>> GetByProductIdAsync(int productId)
    {
        return await _dbContext.ProductReviews
            .Include(r => r.User)
            .Include(r => r.Product)
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }


    public async Task<ProductReview?> GetByIdAsync(int id)
    {
        return await _dbContext.ProductReviews
            .Include(r => r.User)
            .Include(r => r.Product)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<double> GetAverageRatingAsync(int productId)
    {
        return await _dbContext.ProductReviews
            .Where(r => r.ProductId == productId)
            .AverageAsync(r => (double?)r.Rating) ?? 0;

    }

    public async Task<bool> HasUserReviewedAsync(int productId, int userId)
    {
        return await _dbContext.ProductReviews
            .AnyAsync(r => r.ProductId == productId && r.UserId == userId);
    }

    public async Task<ProductReview?> GetByProductAndUserAsync(int productId, int userId)
    {
        return await _dbContext.ProductReviews
            .Include(r => r.User)
            .Include(r => r.Product)
            .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
    }



    public async Task<IEnumerable<float>> GetRatingsByProductIdAsync(int productId)
    {
        return await _dbContext.ProductReviews
            .Where(r => r.ProductId == productId)
            .Select(r => r.Rating)
            .ToListAsync();
    }

    public async Task<IEnumerable<(int ProductId, string ProductName, double AverageRating, int ReviewCount)>>
     GetTopProductsByRatingAsync(int topCount = 3)
    {
        var list = await _dbContext.ProductReviews
            .Include(r => r.Product)
            .GroupBy(r => new { r.ProductId, r.Product.Name })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                AverageRating = g.Average(r => (double)r.Rating),
                ReviewCount = g.Count()
            })
            .OrderByDescending(x => x.AverageRating)
            .Take(topCount)
            .ToListAsync();


        var result = list
            .Select(x => (x.ProductId, x.ProductName, x.AverageRating, x.ReviewCount))
            .ToList();

        return result;



    }

}
