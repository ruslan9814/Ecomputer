using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using test.Database.Repositories.Interfaces;

namespace test.Database.Repositories.Classes;

public abstract class BaseRepository<TEntity>(ApplicationDbContext dbContext, IDistributedCache cache) :
    IBaseRepository<TEntity> where TEntity : class
{

    protected readonly ApplicationDbContext _dbContext = dbContext;
    protected readonly IDistributedCache _cache = cache;

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();//////////////////////////////////////////
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _cache.Remove($"{typeof(TEntity).Name}, {id}");
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();//////////////////////////////////////////
        return true;
    }

    public async Task<TEntity> GetAsync(int id)
    {
        var cacheKey = $"{typeof(TEntity).Name}, {id}";
        var cachedEntity = await _cache.GetAsync(cacheKey);

        if (cachedEntity != null)
        {
            _cache.Refresh(cacheKey);
            return JsonSerializer.Deserialize<TEntity>(cachedEntity);
        }

        var entity = await _dbContext.Set<TEntity>().FindAsync(id);

        if (entity != null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(entity), options);
        }
        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();//////////////////////////////////////////
    }
}
