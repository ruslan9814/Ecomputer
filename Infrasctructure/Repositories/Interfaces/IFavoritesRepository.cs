using Domain.Favorites;

namespace Infrasctructure.Repositories.Interfaces;

public interface IFavoritesRepository : IBaseRepository<Favorite>
{
    Task<Favorite> GetByUserIdAsync(int userId);
    Task<Favorite> GetFavoriteAsync(int Id);
    Task<List<Favorite>> GetAll();
    Task<Favorite?> GetByIdAndUserIdAsync(int favoriteId, int userId);
    Task<List<Favorite>> GetAllByUserIdAsync(int userId);
}
