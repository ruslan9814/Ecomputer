using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

public sealed record GetReviewByIdQuery(int Id, int UserId)
    : IRequest<Result<ProductReviewDto>>;

internal sealed class GetReviewByIdQueryHandler(
    IProductReviewRepository productReviewRepository)
    : IRequestHandler<GetReviewByIdQuery, Result<ProductReviewDto>>
{
    private readonly IProductReviewRepository _productReviewRepository = productReviewRepository;

    public async Task<Result<ProductReviewDto>> Handle(GetReviewByIdQuery request,
        CancellationToken cancellationToken)
    {
        var review = await _productReviewRepository.GetByIdAsync(request.Id);
        if (review is null)
        {
            return Result.Failure<ProductReviewDto>($"Отзыв с ID {request.Id} не найден.");
        }

    
        if (review.UserId != request.UserId)
        {
            return Result.Failure<ProductReviewDto>("Доступ запрещён: отзыв принадлежит другому пользователю.");
        }

        var dto = new ProductReviewDto(
            review.Id,
            review.ProductId,
            review.UserId,
            review.User?.Name ?? "Неизвестно",
            review.Product?.Name ?? "Неизвестно",
            review.Product?.Description ?? "Описание отсутствует",
            review.ReviewText,
            review.Rating,
            review.CreatedAt
        );

        return Result.Success(dto);
    }
}
