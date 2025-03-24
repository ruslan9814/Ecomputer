namespace Infrasctructure.BlackList;

public interface IBlackListService
{
    Task<bool> IsExistsToken(string token);
    Task AddTokenToBlackList(string token, TimeSpan ttl);
    Task RemoveTokenFromBlackList(string token);
}
