using Domain.Favorites;

namespace Infrasctructure.Repositories.Interfaces;

public interface IFavoritesRepository : IBaseRepository<Favorite>
{
    Task<Favorite> GetByUserIdAsync(int userId);
}
