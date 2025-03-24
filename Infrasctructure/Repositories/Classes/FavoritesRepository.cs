using Domain.Favorites;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrasctructure.Repositories.Classes;

public class FavoritesRepository(ApplicationDbContext context, ICacheEntityService cacheEntity) :
    BaseRepository<Favorite>(context, cacheEntity), IFavoritesRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Favorite> GetByUserIdAsync(int userId)
    {
        return await _context.Favorites
            .Include(favorites => favorites.Products)
            .FirstAsync(favorites => favorites.UserId == userId);
    }
}
