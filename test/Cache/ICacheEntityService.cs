using Test.Models.Core;

namespace Test.Cache;

public interface ICacheEntityService
{
    Task<TEntity> GetAsync<TEntity>(int id) where TEntity : EntityBase;
    Task RefreshAsync<TEntity>(TEntity entity) where TEntity : EntityBase;
    Task SetAsync<TEntity>(TEntity entity, TimeSpan? absoluteExpirationRelativeToNow = null) where TEntity : EntityBase;
}