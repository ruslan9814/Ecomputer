using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;
using Domain.Products;
using Microsoft.AspNetCore.Http;
using Infrasctructure.BlobStorage;

namespace Application.Products.Commands;

public sealed record AddProductCommand(
    string Name,
    decimal Price,
    int Quantity,
    bool IsInStock,
    DateTime CreatedDate,
    int CategoryId,
    string Description,
    IFormFile ImageFile
) : IRequest<Result>;

internal sealed class AddProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    IBlobService blobService) : IRequestHandler<AddProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBlobService _blobService = blobService;

    public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var isCategoryExist = await _categoryRepository.IsExistAsync(request.CategoryId);
        if (!isCategoryExist)
        {
            return Result.Failure("Категории не существует.");
        }

        var category = await _categoryRepository.GetAsync(request.CategoryId);

        string imageUrl = string.Empty;
        if (request.ImageFile != null && request.ImageFile.Length > 0)
        {
            imageUrl = await _blobService.UploadFileAsync(request.ImageFile);
        }

        var product = new Product(
            request.Name,
            request.Price,
            request.Quantity,
            request.IsInStock,
            category,
            request.Description,
            imageUrl
        );

        await _productRepository.AddAsync(product);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}

