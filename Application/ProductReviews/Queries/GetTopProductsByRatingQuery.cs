using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
 

namespace Application.ProductReviews.Queries;

public sealed record GetTopProductsByRatingQuery(int TopCount = 3)
        : IRequest<Result<IEnumerable<ProductRatingDto>>>;


internal sealed class GetTopProductsByRatingQueryHandler(
    IProductReviewRepository productReviewRepository)
        : IRequestHandler<GetTopProductsByRatingQuery, Result<IEnumerable<ProductRatingDto>>>
{
    private readonly IProductReviewRepository _productReviewRepository = productReviewRepository;

    public async Task<Result<IEnumerable<ProductRatingDto>>> Handle(
        GetTopProductsByRatingQuery request, CancellationToken cancellationToken)
    {

        var topProducts = await _productReviewRepository.GetTopProductsByRatingAsync(request.TopCount);
        var result = topProducts.Select(p => new ProductRatingDto(p.ProductId, p.ProductName, 
            p.AverageRating, p.ReviewCount));
        return Result.Success(result);
    }
}
