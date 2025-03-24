using Application.Dtos;
using Domain.Products;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Favorite.Quieries;

public sealed record GetFavoritesQuery(int Id) : IRequest<Result<FavoriteDto>>;

internal sealed class GetFavoritesQueryHandler(IFavoritesRepository favoritesRepository) : 
    IRequestHandler<GetFavoritesQuery, Result<FavoriteDto>>
{
    private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;

    public async Task<Result<FavoriteDto>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favoriteIsExists = await _favoritesRepository.IsExistAsync(request.Id);


        if (!favoriteIsExists)
        {
            return Result.Failure<FavoriteDto>("Избранное не найдено.");

        }

        var favorite = await _favoritesRepository.GetAsync(request.Id);

        var response = new FavoriteDto
        (
            favorite.Id,
            favorite.Products.Select(x => new ProductDto
            (
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.IsInStock,
                x.CreatedDate,
                x.CategoryId
            //new CategoryDto(x.Category.Id, x.Category.Name)
            )).ToList(),
            favorite.UserId_FK
        );

        return Result.Success(response);
    }
}
