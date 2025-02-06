using test.Common;
using test.CQRS.Dtos;
using Test.Database.Repositories.Interfaces;
using Test.Models;

namespace test.CQRS.Products.Queries;

public sealed record GetProducts() : IRequest<Result<ProductDto>>;

public sealed class GetProductsHandler(IProductRepository productRepository) : IRequestHandler<GetProducts, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result<ProductDto>> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAllAsync();

        if (product is null)
        {
            return (Result<ProductDto>)Result.Failure("Нет доступных продуктов.");
        }

        var productDtos = product.Select(product => new ProductDto(
               product.Id,
               product.Name,
               product.Description,
               product.Price,
               product.IsInStock,
               product.CreatedDate
           )).ToList();

        return (Result<ProductDto>)Result.Success;
    }
}
