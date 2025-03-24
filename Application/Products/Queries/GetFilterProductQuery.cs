using Domain.Common;
using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Products.Queries;

public sealed record GetFilterProductQuery(
    string? Name,
    decimal MinPrice,
    decimal MaxPrice,
    bool IsInStock,
    int CategoryId,//////////////////////////podumat
    int PageNumber = 1,
    int PageSize = 8) : IRequest<Result<ResultPage<ProductDto>>>;

internal sealed class GetFilterProductQueryHandler(IProductRepository productRepository) :
    IRequestHandler<GetFilterProductQuery, Result<ResultPage<ProductDto>>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result<ResultPage<ProductDto>>> Handle(GetFilterProductQuery request, 
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetFilteredProductsAsync(
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            request.IsInStock,
            request.CategoryId
        );

        var countProducts = await _productRepository.GetProductCountAsync(
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            request.IsInStock);


        var productsDto = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.IsInStock,
            p.CreatedDate,
            p.CategoryId
        //new CategoryDto(p.Category.Id, p.Category.Name)
        )).ToList();


        var pageResult = new ResultPage<ProductDto>(
            request.PageSize,
            request.PageNumber,
            countProducts,
            productsDto
        );


        return Result.Success(pageResult);
    }
}
