using test.Common;
using test.CQRS.Dtos;
using Test.Database.Repositories.Classes;
using Test.Database.Repositories.Interfaces;
using Test.Models;

namespace test.CQRS.Products.Queries;

public sealed record GetFilterProduct(
    int Id,
    string? Name,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? Description,
    int? Quantity,
    bool? IsInStock,
    int pageNumber,
    int pageSize) : IRequest<Result<GetFilterProductPageDto>>;

public sealed class GetFilterProductHandler(IProductRepository productRepository) : IRequestHandler<GetFilterProduct, Result<GetFilterProductPageDto>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result<GetFilterProductPageDto>> Handle(GetFilterProduct request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetFilteredProductsCountAsync(
            request.Id,
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            request.Description,
            request.Quantity,
            request.IsInStock
        );

        // Если продукты не найдены
        if (!products.Any())
        {
            return Result.Failure<GetFilterProductPageDto>("Продукты не найдены.");
        }

        // Преобразуем продукты в DTO
        var productDtos = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.IsInStock,
            p.CreatedDate
        )).ToList();

        // Получаем общее количество продуктов, которые соответствуют фильтрам
        var totalCount = await _productRepository.GetFilteredProductsCountAsync(
            request.Id,
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            request.Description,
            request.Quantity,
            request.IsInStock
        );

        // Вычисляем общее количество страниц
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        // Создаем DTO для пагинированного ответа
        var pagedResult = new GetFilterProductPageDto(
            productDtos,
            totalCount,
            totalPages
        );

        // Возвращаем результат с пагинацией
        return Result<GetFilterProductPageDto>.Success(pagedResult);
    }
}
