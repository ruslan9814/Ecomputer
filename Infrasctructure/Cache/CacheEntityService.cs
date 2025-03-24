using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrasctructure.Cache;

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

    public async Task DeleteAsync<TEntity>(int id) 
        where TEntity : EntityBase
    {
       var key = GetKey<TEntity>(id);
       await _cache.RemoveAsync(key);
    }



    //public async Task<bool> IsExistsToken(string token) //////////эти методы добавить в сам middleware blackList
    //{
    //    var key = $"Token-{token}";
    //    var cachedData = await _cache.GetStringAsync(key);
    //    return cachedData is not null;
    //}

    //public async Task SetTokenAsync(string token, TimeSpan? absoluteExpirationRelativeToNow = null)//////////эти методы добавить в сам middleware blackList
    //{
    //    var key = $"Token-{token}";
    //    var options = new DistributedCacheEntryOptions
    //    {
    //        AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(5),
    //    };
    //    await _cache.SetStringAsync(key, token, options);
    //}

    //public async Task DeleteTokenAsync(string token)//////////эти методы добавить в сам middleware blackList
    //{
    //    var key = $"Token-{token}";
    //    await _cache.RemoveAsync(key);
    //}

}
