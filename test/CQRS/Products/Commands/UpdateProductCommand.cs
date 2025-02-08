using test.Common;
using Test.Database.Repositories.Interfaces;
using test.Database.Service.UnitOfWork;

namespace test.CQRS.Products.Commands;

public sealed record UpdateProductCommand(
    int ProductId,
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    bool IsInStock
) : IRequest<Result>;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {

        var productIsExist = await _productRepository.IsExistAsync(request.ProductId);

        if (!productIsExist)
        {
            return Result.Failure("Продукт не найден.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);

        var updateResult = product.Update(
            request.Name,
            request.Price,
            request.Quantity,
            request.IsInStock,
            request.Description
        );

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        await _productRepository.UpdateAsync(product);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
