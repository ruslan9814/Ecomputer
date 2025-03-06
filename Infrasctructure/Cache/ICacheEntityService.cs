namespace Infrasctructure.Cache;

public interface ICacheEntityService
{
    Task<TEntity> GetAsync<TEntity>(int id) where TEntity : EntityBase;
    Task RefreshAsync<TEntity>(TEntity entity) where TEntity : EntityBase;
    Task SetAsync<TEntity>(TEntity entity, TimeSpan? absoluteExpirationRelativeToNow = null) where TEntity : EntityBase;
    Task DeleteAsync<TEntity>(int id) where TEntity : EntityBase;
}