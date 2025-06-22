using Infrasctructure.BlobStorage;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application.Products.Commands;

public sealed record UpdateProductCommand(
    int ProductId,
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    bool IsInStock,
    int CategoryId,
    IFormFile? ImageFile
) : IRequest<Result>;

internal sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    IBlobService blobService) : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBlobService _blobService = blobService;

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productIsExist = await _productRepository.IsExistAsync(request.ProductId);
        if (!productIsExist)
        {
            return Result.Failure("Продукт не найден.");
        }

        var categoryIsExist = await _categoryRepository.IsExistAsync(request.CategoryId);
        if (!categoryIsExist)
        {
            return Result.Failure("Категория не найдена.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);
        var category = await _categoryRepository.GetAsync(request.CategoryId);

        string imageUrl = product.ImageUrl;
        if (request.ImageFile != null && request.ImageFile.Length > 0)
        {
            imageUrl = await _blobService.UploadFileAsync(request.ImageFile);
        }

        var updateResult = product.Update(
            request.Name,
            request.Price,
            request.Quantity,
            request.IsInStock,
            request.Description,
            category,
            imageUrl
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

