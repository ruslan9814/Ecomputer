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

    public async Task<Favorite?> GetByUserIdAsync(int userId)
    {
        return await _context.Favorites
            .Include(f => f.FavoriteProducts)
                .ThenInclude(fp => fp.Product)
                .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(f => f.UserId == userId);
    }

    public async Task<Favorite?> GetFavoriteAsync(int id)
    {
        return await _context.Favorites
            .Include(f => f.FavoriteProducts)
                .ThenInclude(fp => fp.Product)
                .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<Favorite>> GetAll()
    {
        return await _dbContext.Favorites
            .Include(f => f.FavoriteProducts)
                .ThenInclude(fp => fp.Product)
                .ThenInclude(p => p.Category)
            .ToListAsync();
    }

    public async Task<Favorite?> GetByIdAndUserIdAsync(int favoriteId, int userId)
    {
        return await _dbContext.Favorites
            .Include(f => f.FavoriteProducts)
                .ThenInclude(fp => fp.Product)
                .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(f => f.Id == favoriteId && f.UserId == userId);
    }

    public async Task<List<Favorite>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.FavoriteProducts)
                .ThenInclude(fp => fp.Product)
                    .ThenInclude(p => p.Category)
            .ToListAsync();
    }


}
