using Infrasctructure.Database;
using Microsoft.EntityFrameworkCore;
using Domain.Categories;
using Infrasctructure.Cache;
using Infrasctructure.Repositories.Interfaces;

namespace Infrasctructure.Repositories.Classes;

public class CategoryRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<Category>(dbContext, cache), ICategoryRepository
{
    public async Task<bool> IsExistByNameAsync(string name) =>
        await _dbContext
            .Set<Category>()
            .AnyAsync(x => x.Name == name);

    public async Task<List<Category>> GetAll(bool includeRelated = false)
    {
        var query = _dbContext.Set<Category>().AsQueryable();
        if (includeRelated)
        {
            query = query.Include(c => c.Products);
        }

        var categories = await query.ToListAsync();
        return categories;
    }

    public async Task<Category?> GetAsync(int id, bool includeRelated)
    {
        var query = _dbContext.Set<Category>().AsQueryable();

        if (includeRelated)
        {
            query = query.Include(c => c.Products);
        }

        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }

}
