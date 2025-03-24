using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Favorite.Command;

public sealed record DeleteFavoritesCommand(int UserId, int ProductId) : IRequest<Result>;

internal sealed class DeleteFavoritesCommandHandler(
    IFavoritesRepository favoritesRepository, IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteFavoritesCommand, Result>
{
    private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(DeleteFavoritesCommand request, 
        CancellationToken cancellationToken)
    {
        var favoritesIsExists = await _favoritesRepository.IsExistAsync(request.UserId);
        if (!favoritesIsExists)
        {
            return Result.Failure("Избранное не найдено.");
        }

        var favorites = await _favoritesRepository.GetAsync(request.UserId);

        var productISExists = await _productRepository.IsExistAsync(request.ProductId);
        if (!productISExists)
        {
            return Result.Failure("Продукт не найден.");
        }

        var product = await _productRepository.GetAsync(request.ProductId); 

        favorites.RemoveProduct(product.Id);
        await _favoritesRepository.UpdateAsync(favorites);
        await _unitOfWork.Commit();
        return Result.Success();
    }
}
