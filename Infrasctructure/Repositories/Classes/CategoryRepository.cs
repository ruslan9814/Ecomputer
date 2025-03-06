using Infrasctructure.Database;
using Domain.Carts;
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
}
