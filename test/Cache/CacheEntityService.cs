using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Test.Models.Core;

namespace Test.Cache;

public class CacheEntityService(IDistributedCache cache) : ICacheEntityService
{

    private readonly IDistributedCache _cache = cache;
    public async Task<TEntity> GetAsync<TEntity>(int id)
        where TEntity : EntityBase
    {
        var key = GetKey<TEntity>(id);

        var cachedData = await _cache.GetStringAsync(key);

        return JsonSerializer.Deserialize<TEntity>(cachedData!)!;
    }


    public async Task SetAsync<TEntity>(TEntity entity, TimeSpan? absoluteExpirationRelativeToNow = null) where
        TEntity : EntityBase
    {
        var key = GetKey<TEntity>(entity.Id);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(5),
        };

        var serializedEntity = JsonSerializer.Serialize(entity);

        await _cache.SetStringAsync(key, serializedEntity, options);
    }

    private static string GetKey<TEntity>(int id) =>
        $"{typeof(TEntity)}-{id}";

    // Обновление кэша (продлевает срок хранения)
    public async Task RefreshAsync<TEntity>(TEntity entity)
        where TEntity : EntityBase
    {
        var key = GetKey<TEntity>(entity.Id);
        await _cache.RefreshAsync(key);
    }
}
