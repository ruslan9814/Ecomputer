using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Products.Queries;

public sealed record GetAllProductsQuery() : IRequest<Result<IEnumerable<ProductDto>>>;

internal sealed class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAll(includeRelated: true);

        if (products is null || !products.Any())
        {
            return Result.Failure<IEnumerable<ProductDto>>("Продукты не найдены.");
        }

        var productDtos = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.IsInStock,
            p.CreatedDate,
            p.Quantity,
            p.CategoryId,
            p.Category?.Name ?? string.Empty,
            p.Rating,
            p.ImageUrl 
        )).ToList();

        return Result.Success<IEnumerable<ProductDto>>(productDtos);
    }
}
