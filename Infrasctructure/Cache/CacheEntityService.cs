using Infrasctructure.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CacheEntityService : ICacheEntityService
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly TimeSpan _defaultCacheDuration;

    public CacheEntityService(IDistributedCache cache, IConfiguration configuration)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        _defaultCacheDuration = TimeSpan.FromMinutes(
            configuration.GetValue<int>("Cache:DefaultDurationMinutes", 5));
    }

    public async Task<TEntity?> GetAsync<TEntity>(
        int id,
        CancellationToken cancellationToken = default)
        where TEntity : EntityBase
    {
        var key = GetKey<TEntity>(id);
        var cachedData = await _cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
        {
            return null;
        }

        return JsonSerializer.Deserialize<TEntity>(cachedData, _jsonOptions);
    }

    public async Task SetAsync<TEntity>(
        TEntity entity,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        CancellationToken cancellationToken = default)
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(entity);

        var key = GetKey<TEntity>(entity.Id);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? _defaultCacheDuration,
            SlidingExpiration = absoluteExpirationRelativeToNow == null ? TimeSpan.FromMinutes(2) : null
        };

        var serializedEntity = JsonSerializer.Serialize(entity, _jsonOptions);
        await _cache.SetStringAsync(key, serializedEntity, options, cancellationToken);
    }

    public async Task RefreshAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default)
        where TEntity : EntityBase
    {
        var key = GetKey<TEntity>(entity.Id);
        await _cache.RefreshAsync(key, cancellationToken);
    }

    public async Task DeleteAsync<TEntity>(
        int id,
        CancellationToken cancellationToken = default)
        where TEntity : EntityBase
    {
        var key = GetKey<TEntity>(id);
        await _cache.RemoveAsync(key, cancellationToken);
    }

    public async Task<bool> ExistsAsync<TEntity>(int id, CancellationToken cancellationToken = default)
        where TEntity : EntityBase
    {
        var key = GetKey<TEntity>(id);
        var cachedData = await _cache.GetStringAsync(key, cancellationToken);
        return !string.IsNullOrEmpty(cachedData);
    }

    private static string GetKey<TEntity>(int id) where TEntity : EntityBase
    {
        return $"{typeof(TEntity).Name}-{id}";
    }
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

