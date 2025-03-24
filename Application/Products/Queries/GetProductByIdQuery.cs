using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Products.Queries;

public sealed record GetProductByIdQuery(int Id) : IRequest<Result<ProductDto>>;

internal sealed class GetProductByIdQueryHandler(IProductRepository productRepository) : 
    IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var productIsExist = await _productRepository.IsExistAsync(request.Id);

        if (!productIsExist)
        {
            return Result.Failure<ProductDto>(
                $"Продукт с ID {request.Id} не найден.");
        }

        var product = await _productRepository.GetAsync(request.Id);

        var response = new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.IsInStock,
            product.CreatedDate,
            new CategoryDto(product.Category.Id, product.Category.Name)
        );

        return Result.Success(response);
    }
}
