using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;
using Domain.Products;

namespace Application.Products.Commands;

public sealed record AddProductCommand(
    string Name,
    decimal Price,
    int Quantity,
    bool IsInStock,
    DateTime CreatedDate,
    int CategoryId,
    string Description
) : IRequest<Result>;

internal sealed class AddProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository) : IRequestHandler<AddProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var isCategoryExist = await _productRepository.IsExistAsync(request.CategoryId);
        if (!isCategoryExist)
        {
            return Result.Failure("Категории не существует.");
        }

        var category = await _categoryRepository.GetAsync(request.CategoryId);


        var product = new Product(
                request.Name,
                request.Price,
                request.Quantity,
                request.IsInStock,
                request.CreatedDate,
                category,
                request.Description);

        await _productRepository.AddAsync(product);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}