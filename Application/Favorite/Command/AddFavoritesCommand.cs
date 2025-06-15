using Domain.Products;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Favorite.Command;

public sealed record AddFavoritesCommand(
    int ProductId
) : IRequest<Result>;

internal sealed class AddFavoritesCommandHandler(
    IFavoritesRepository favoritesRepository,
    IProductRepository productRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<AddFavoritesCommand, Result>
{
    private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddFavoritesCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId <= 0)
        {
            return Result.Failure("User is not authenticated.");
        }

        var favorites = await _favoritesRepository.GetAsync(userId);
        if (favorites is null)
        {
            return Result.Failure("Favorites not found.");
        }

        var product = await _productRepository.GetAsync(request.ProductId);
        if (product is null)
        {
            return Result.Failure("Product not found.");
        }

        if (favorites.ContainsProduct(product.Id))
        {
            return Result.Failure("Product already in favorites.");
        }

        favorites.AddProduct(product);
        await _favoritesRepository.UpdateAsync(favorites);
        await _unitOfWork.Commit();

        return Result.Success();
    }

}
