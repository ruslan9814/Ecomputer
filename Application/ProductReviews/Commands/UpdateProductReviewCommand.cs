using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.ProductReviews.Commands;

public sealed record UpdateProductReviewCommand(
    int Id,
    string ReviewText,
    int? Rating = null
) : IRequest<Result>;

internal sealed class UpdateProductReviewCommandHandler(
    IProductReviewRepository productReviewRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser)  
    : IRequestHandler<UpdateProductReviewCommand, Result>
{
    private readonly IProductReviewRepository _productReviewRepository = productReviewRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(UpdateProductReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _productReviewRepository.GetByIdAsync(request.Id);

        if (review is null)
        {
            return Result.Failure("Отзыв не найден.");
        }

        if (review.UserId != _currentUser.UserId)
        {
            return Result.Failure("Нет доступа для редактирования этого отзыва.");
        }

        review.ReviewText = request.ReviewText;

        if (request.Rating is not null)
        {
            review.Rating = request.Rating.Value;
        }

        await _productReviewRepository.UpdateAsync(review);

        var product = await _productRepository.GetAsync(review.ProductId);

        if (product is not null)
        {
            var ratings = await _productReviewRepository.GetRatingsByProductIdAsync(product.Id);
            product.RecalculateRating(ratings);
            await _productRepository.UpdateAsync(product);
        }

        await _unitOfWork.Commit();

        return Result.Success("Отзыв успешно обновлён.");
    }
}
