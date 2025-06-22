using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

public sealed record GetReviewsByProductIdQuery(int ProductId)
    : IRequest<Result<List<ProductReviewDto>>>;

internal sealed class GetReviewsByProductIdQueryHandler(
    IProductReviewRepository productReviewRepository,
    ILogger<GetReviewsByProductIdQueryHandler> logger)
    : IRequestHandler<GetReviewsByProductIdQuery, Result<List<ProductReviewDto>>>
{
    private readonly IProductReviewRepository _productReviewRepository = productReviewRepository;
    private readonly ILogger<GetReviewsByProductIdQueryHandler> _logger = logger;

    public async Task<Result<List<ProductReviewDto>>> Handle(GetReviewsByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получаем отзывы для продукта с Id = {ProductId}", request.ProductId);

        var reviews = await _productReviewRepository.GetByProductIdAsync(request.ProductId);

        if (reviews is null || !reviews.Any())
        {
            _logger.LogInformation("Отзывы для продукта с Id = {ProductId} не найдены", request.ProductId);
            return Result.Success(new List<ProductReviewDto>());
        }

        var dtoList = reviews.Select(r =>
        {
            var userName = r.User?.Name ?? "Неизвестный пользователь";
            var productName = r.Product?.Name ?? "Неизвестный продукт";
            var productDescription = r.Product?.Description ?? "Описание отсутствует";

            _logger.LogInformation("Отзыв Id = {ReviewId}, Пользователь = {UserName}, Рейтинг = {Rating}", r.Id, userName, r.Rating);

            return new ProductReviewDto(
                r.Id,
                r.ProductId,
                r.UserId,
                userName,
                productName,
                productDescription,
                r.ReviewText,
                r.Rating,
                r.CreatedAt);
        }).ToList();

        return Result.Success(dtoList);
    }
}
