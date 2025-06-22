using Domain.Products;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.ProductReviews.Commands;

public sealed record DeleteProductReviewCommand(int Id) : IRequest<Result>;

internal sealed class DeleteProductReviewCommandHandler(
    IProductReviewRepository productReviewRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<DeleteProductReviewCommand, Result>
{
    private readonly IProductReviewRepository _productReviewRepository = productReviewRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(DeleteProductReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _productReviewRepository.GetByIdAsync(request.Id);

        if (review is null)
        {
            return Result.Failure("Отзыв не найден.");
        }

        if (review.UserId != _currentUser.UserId)
        {
            return Result.Failure("Нет доступа для удаления этого отзыва.");
        }

        await _productReviewRepository.DeleteAsync(review.Id);

        var ratings = await _productReviewRepository.GetRatingsByProductIdAsync(review.ProductId);

        var product = await _productRepository.GetAsync(review.ProductId);
        if (product is not null)
        {
            product.RecalculateRating(ratings);
            await _productRepository.UpdateAsync(product);
        }

        await _unitOfWork.Commit();

        return Result.Success("Отзыв успешно удалён и рейтинг обновлён.");
    }
}
