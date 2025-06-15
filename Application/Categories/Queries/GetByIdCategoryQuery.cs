using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Categories.Queries;

public sealed record GetByIdCategoryQuery(int Id) : IRequest<Result<CategoryDto>>;

internal sealed class GetByIdCategoryQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetByIdCategoryQuery, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result<CategoryDto>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryIsExists = await _categoryRepository.IsExistAsync(request.Id);
        if (!categoryIsExists)
        {
            return Result.Failure<CategoryDto>("Категория не найдена.");
        }

        var category = await _categoryRepository.GetAsync(request.Id, includeRelated: true);

        if (category is null)
        {
            return Result.Failure<CategoryDto>("Категория не найдена.");
        }

        var productDtos = category.Products?
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.IsInStock,
                p.CreatedDate,
                p.Quantity,
                p.CategoryId,
                p.Category?.Name ?? string.Empty,
                p.Rating
            ))
            .ToList();

        var response = new CategoryDto(
            category.Id,
            category.Name,
            productDtos ?? []
        );

        return Result.Success(response);
    }
}
