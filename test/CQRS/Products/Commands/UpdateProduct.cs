using test.Common;
using Test.Models;
using Test.Database.Repositories.Interfaces;
using test.Database.DbService;

namespace test.CQRS.Products.Commands
{
    public sealed record UpdateProduct(
        int ProductId,
        string Name,
        string Description,
        decimal Price,
        bool IsInStock
    ) : IRequest<Result>;

    public sealed class UpdateProductHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateProduct, Result>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetAsync(request.ProductId);

            if (existingProduct is null)
            {
                return Result.Failure("Продукт не найден.");
            }

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.IsInStock = request.IsInStock;

            await _productRepository.UpdateAsync(existingProduct);
            await _unitOfWork.Commit();

            return Result.Success;
        }
    }
}
