namespace Infrasctructure.Jwt
{
    public interface IJwtService
    {
        Task<string> GetToken(string username, string role, string email);
        string GetRefreshToken();
    }
}