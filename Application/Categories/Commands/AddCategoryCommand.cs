using Domain.Categories;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Categories.Commands;

public sealed record AddCategoryCommand(string Name) : IRequest<Result>;

internal sealed class AddCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository
) : IRequestHandler<AddCategoryCommand, Result>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryIsExist = await _categoryRepository.IsExistByNameAsync(request.Name);

        if (categoryIsExist)
        {
            return Result.Failure("Категория с таким названием уже существует.");
        }

        var category = new Category(request.Name);

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
