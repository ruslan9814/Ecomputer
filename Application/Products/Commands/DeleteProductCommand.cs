using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Products.Commands;

public sealed record DeleteProductCommand(int ProductId) : IRequest<Result>;

internal sealed class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteProductCommand request, 
        CancellationToken cancellationToken)
    {
        var productIsExist = await _productRepository.IsExistAsync(request.ProductId);

        if (!productIsExist)
        {
            return Result.Failure("Продукт не найден.");
        }

        await _productRepository.DeleteAsync(request.ProductId);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
