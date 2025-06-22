
using Domain.Products;

namespace Infrasctructure.Repositories.Interfaces;

public interface IProductReviewRepository : IBaseRepository<ProductReview>
{
    Task<List<ProductReview>> GetByProductIdAsync(int productId);
    Task<ProductReview?> GetByIdAsync(int reviewId);
    Task<ProductReview?> GetByProductAndUserAsync(int productId, int userId);
    Task<IEnumerable<float>> GetRatingsByProductIdAsync(int productId);

    Task<IEnumerable<(int ProductId, string ProductName, double AverageRating, int ReviewCount)>>
        GetTopProductsByRatingAsync(int topCount = 3);

}

