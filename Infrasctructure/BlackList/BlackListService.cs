using Microsoft.Extensions.Caching.Distributed;

namespace Infrasctructure.BlackList;

public sealed class BlackListService(IDistributedCache cache) : IBlackListService
{
    private readonly IDistributedCache _cache = cache;

    public async Task AddTokenToBlackList(string token, TimeSpan ttl)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };
        await _cache.SetStringAsync(token, "blacklisted", options);
    }

    public async Task<bool> IsExistsToken(string token) //////////эти методы добавить в сам middleware blackList
    {
        var cachedData = await _cache.GetStringAsync(token);
        return cachedData is not null;
    }

    public async Task RemoveTokenFromBlackList(string token)
    {
        await _cache.RemoveAsync(token);
    }
}

