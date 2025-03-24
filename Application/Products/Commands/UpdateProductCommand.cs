using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Products.Commands;

public sealed record UpdateProductCommand(
    int ProductId,
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    bool IsInStock,
    int CategoryId
) : IRequest<Result>;

internal sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository) : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateProductCommand request, 
        CancellationToken cancellationToken)
    {

        var productIsExist = await _productRepository.IsExistAsync(request.ProductId);

        if (!productIsExist)
        {
            return Result.Failure("Продукт не найден.");
        }

        var categoryIsExist = await _productRepository.IsExistAsync(request.CategoryId);

        if (!categoryIsExist) 
        {
            return Result.Failure("Категория не найдена.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);
        var category = await _categoryRepository.GetAsync(request.CategoryId);

        var updateResult = product.Update(
            request.Name,
            request.Price,
            request.Quantity,
            request.IsInStock,
            request.Description,
            category
        );

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        await _productRepository.UpdateAsync(product);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
