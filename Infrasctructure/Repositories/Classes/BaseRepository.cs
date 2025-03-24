using Microsoft.EntityFrameworkCore;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;
using OpenQA.Selenium;

namespace Infrasctructure.Repositories.Classes;

// TODO: Сделать базовый класс и
// создать класс для сохранения
// в базу данных и вызывать этот класс в endpoints,
//также создать папку cacheService и там класс чтобы вызывать через нее JsonSerialize
public abstract class BaseRepository<TEntity>(ApplicationDbContext dbContext, ICacheEntityService cache) :
    IBaseRepository<TEntity> where TEntity : EntityBase
{

    protected readonly ApplicationDbContext _dbContext = dbContext;
    protected readonly ICacheEntityService _cache = cache;

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _cache.SetAsync(entity); 
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(id);
        if (entity is null)
            return false;

        await _cache.DeleteAsync<TEntity>(id);
        _dbContext.Set<TEntity>().Remove(entity);
        return true;
    }

    public async Task<TEntity> GetAsync(int id, bool includeRelated = false)
    {
    
        //var cachedEntity = await _cache.GetAsync<TEntity>(id);
        //if (cachedEntity is not null)
        //{
        //    await _cache.RefreshAsync(cachedEntity);
        //    return cachedEntity;
        //}


        var query = _dbContext.Set<TEntity>().AsQueryable();

        if (includeRelated)
            query = IncludeRelated(query);

        var entity = await query.FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException(typeof(TEntity).Name);

        await _cache.SetAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _cache.SetAsync(entity);
    }

    public async Task<bool> IsExistAsync(int id) =>
        await _dbContext.Set<TEntity>().AnyAsync(e => e.Id == id);

    protected virtual IQueryable<TEntity> IncludeRelated(IQueryable<TEntity> query)
    {
        return query;
    }

}
