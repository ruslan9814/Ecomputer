using Domain.Favorites;
using Domain.Products;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Favorite.Command;

public sealed record AddFavoritesCommand( /// сделать добавление  и удаление прродукта из избранного а сам класс создавать в констукторе юзера
    int UserId,
    int ProductId,
    ICollection<Product> Products
) : IRequest<Result>;

internal sealed class AddFavoritesCommandHandler(IFavoritesRepository favoritesRepository, 
    IProductRepository productRepository, IUnitOfWork unitOfWork) :
    IRequestHandler<AddFavoritesCommand, Result>
{
    private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddFavoritesCommand request, 
        CancellationToken cancellationToken)
    {
        var favoritesIsExist = await _favoritesRepository.IsExistAsync(request.UserId);
        if (!favoritesIsExist)
        {
            return Result.Failure("Favorites already exist.");
        }
        var favorites = await _favoritesRepository.GetAsync(request.UserId);

        var productIsExists = await _productRepository.IsExistAsync(request.ProductId);
        if (!productIsExists)
        {
            return Result.Failure("Product not found.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);
        favorites.AddProduct(product);
        await _favoritesRepository.UpdateAsync(favorites);
        await _unitOfWork.Commit();

        return Result.Success();
    }

}
