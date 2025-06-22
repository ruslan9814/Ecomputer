using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Categories.Queries;

public sealed class GetAllCategoriesQuery() : IRequest<Result<IEnumerable<CategoryDto>>>;

internal sealed class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAll(includeRelated: true);
        if (categories is null)
        {
            return Result.Failure<IEnumerable<CategoryDto>>("Категории не найдены.");
        }
        var categoryDtos = categories
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Products?.Select(p => new ProductDto(
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
                )).ToList() ?? []
            ))
            .ToList();
        return Result.Success<IEnumerable<CategoryDto>>(categoryDtos);
    }
}
