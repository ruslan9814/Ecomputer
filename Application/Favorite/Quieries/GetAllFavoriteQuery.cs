using Application.Dtos;
using Infrasctructure.CurrentUser;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Favorite.Quieries;

public sealed record GetAllFavoriteQuery()
    : IRequest<Result<IEnumerable<FavoriteDto>>>;

internal sealed class GetAllFavoriteQueryHandler(IFavoritesRepository favoritesRepository, 
    ICurrentUserService currentUser)
    : IRequestHandler<GetAllFavoriteQuery, Result<IEnumerable<FavoriteDto>>>
{
    private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result<IEnumerable<FavoriteDto>>> Handle(GetAllFavoriteQuery request,
     CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId <= 0)
        {
            return Result.Failure<IEnumerable<FavoriteDto>>("Пользователь не авторизован.");
        }

        var favorites = await _favoritesRepository.GetAllByUserIdAsync(userId);

        if (favorites is null || favorites.Count == 0)
        {
            return Result.Failure<IEnumerable<FavoriteDto>>("Избранное не найдено.");
        }

        var response = favorites.Select(favorite => new FavoriteDto
        (
            favorite.Id,
            favorite.FavoriteProducts.Select(fp => new ProductDto
            (
                fp.Product.Id,
                fp.Product.Name,
                fp.Product.Description,
                fp.Product.Price,
                fp.Product.IsInStock,
                fp.Product.CreatedDate,
                fp.Product.Quantity,
                fp.Product.CategoryId,
                fp.Product.Category?.Name ?? string.Empty,
                fp.Product.Rating,
                fp.Product.ImageUrl
            )).ToList(),
            favorite.UserId
        )).ToList();

        return Result.Success<IEnumerable<FavoriteDto>>(response);
    }

}
