using Infrasctructure.UnitOfWork;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Categories.Commands;

public sealed record DeleteCategoryCommand(int Id) : IRequest<Result>;

internal sealed class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : 
    IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entityIsExists = await _categoryRepository.IsExistAsync(request.Id);
        if (!entityIsExists)
        {
            return Result.Failure($"Category with id {request.Id} not found.");
        }

        var entity = await _categoryRepository.GetAsync(request.Id);
        await _categoryRepository.DeleteAsync(entity.Id);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
