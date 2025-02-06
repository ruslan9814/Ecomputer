using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;

namespace test.CQRS.Products.Commands
{
    public sealed record DeleteProduct(int ProductId) : IRequest<Result>;

    public sealed class DeleteProductHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteProduct, Result>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var productIsExist = await _productRepository.IsExistAsync(request.ProductId);

            if (!productIsExist)
            {
                return Result.Failure("Продукт не найден.");
            }

            var product = await _productRepository.GetAsync(request.ProductId);

            await _productRepository.DeleteAsync(request.ProductId);
            await _unitOfWork.Commit();

            return Result.Success;
        }
    }
}
