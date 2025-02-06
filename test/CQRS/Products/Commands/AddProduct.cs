using test.Common;
using Test.Models;
using Test.Database.Repositories.Interfaces;
using test.Database.DbService;

namespace test.CQRS.Products.Commands
{
    public sealed record AddProduct(
        string Name,
        string Description,
        decimal Price,
        bool IsInStock
    ) : IRequest<Result>;

    public sealed class AddProductHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<AddProduct, Result>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(AddProduct request, CancellationToken cancellationToken)
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                IsInStock = request.IsInStock,
                CreatedDate = DateTime.UtcNow
            };

            await _productRepository.AddAsync(newProduct);
            await _unitOfWork.Commit();

            return Result.Success;
        }
    }
}
