using Microsoft.EntityFrameworkCore;
using Test.Cache;
using Test.Database.Repositories.Interfaces;
using Test.Models.Core;

namespace Test.Database.Repositories.Classes;

// TODO: Сделать базовый класс и
// создать класс для сохранения
// в базу данных и вызывать этот класс в endpoints,
//также создать папку cacheService и там класс чтобы вызывать через нее JsonSerialize
public abstract class BaseRepository<TEntity>(ApplicationDbContext dbContext, ICacheEntityService cache) :
    IBaseRepository<TEntity> where TEntity : EntityBase
{

    protected readonly ApplicationDbContext _dbContext = dbContext;
    protected readonly ICacheEntityService _cache = cache;

    public async Task AddAsync(TEntity entity) => 
        await _dbContext.Set<TEntity>().AddAsync(entity);

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _cache.Remove(entity);
        _dbContext.Set<TEntity>().Remove(entity);
        return true;
    }


    public async Task<TEntity> GetAsync(int id)
    {
        var entity = await _cache.GetAsync<TEntity>(id);

        if (entity is not null)
        {
            await _cache.RefreshAsync(entity);
            return entity;
        }

        entity = await _dbContext.Set<TEntity>().FindAsync(id)
            ?? throw new NullReferenceException();

        await _cache.SetAsync(entity);

        return entity;

    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _cache.SetAsync(entity);
    }

    public async Task<bool> IsExistAsync(int id) =>
        await _dbContext.Set<TEntity>().AnyAsync(u => u.Id == id); 
}
