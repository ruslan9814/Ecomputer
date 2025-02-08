using test.Common;
using Test.Models;
using Test.Database.Repositories.Interfaces;
using test.Database.Service.UnitOfWork;

namespace test.CQRS.Products.Commands;

public sealed record AddProductCommand(
    string Name,
    string Description,
    decimal Price,
    bool IsInStock
) : IRequest<Result>;

public sealed class AddProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<AddProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            IsInStock = request.IsInStock,
            CreatedDate = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
