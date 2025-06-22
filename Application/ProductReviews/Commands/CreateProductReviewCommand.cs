using Domain.Products;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.ProductReviews.Commands;

public sealed record CreateProductReviewCommand(
    int ProductId,
    int Rating,
    string ReviewText
) : IRequest<Result>;

internal sealed class CreateProductReviewCommandHandler(
    IProductReviewRepository productReviewRepository,
    IProductRepository productRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) 
    : IRequestHandler<CreateProductReviewCommand, Result>
{
    private readonly IProductReviewRepository _productReviewRepository = productReviewRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(CreateProductReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var userExists = await _userRepository.IsExistAsync(userId);
        if (!userExists)
        {
            return Result.Failure("Пользователь не найден.");
        }

        var productExists = await _productRepository.IsExistAsync(request.ProductId);
        if (!productExists)
        {
            return Result.Failure("Продукт не найден.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);
        if (product is null)
        {
            return Result.Failure("Продукт не найден (null).");
        }

        var existingReview = await _productReviewRepository
            .GetByProductAndUserAsync(request.ProductId, userId);

        if (existingReview is not null)
        {
            existingReview.ReviewText = request.ReviewText;
            existingReview.Rating = request.Rating;
            existingReview.CreatedAt = DateTime.UtcNow;

            await _productReviewRepository.UpdateAsync(existingReview);
        }
        else
        {
            var newReview = new ProductReview(
                id: 0,
                productId: request.ProductId,
                userId: userId,
                reviewText: request.ReviewText,
                rating: request.Rating
            );

            await _productReviewRepository.AddAsync(newReview);
        }

        var allRatings = await _productReviewRepository.GetRatingsByProductIdAsync(request.ProductId);
        var ratingResult = product.RecalculateRating(allRatings);

        if (ratingResult.IsFailure)
        {
            return ratingResult;
        }

        await _productRepository.UpdateAsync(product);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}