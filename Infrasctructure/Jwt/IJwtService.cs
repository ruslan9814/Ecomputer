namespace Infrasctructure.Jwt
{
    public interface IJwtService
    {
        Task<string> GetToken(int id, string username, string role, string email);
        string GenerateRefreshToken();
        int GetUserIdFromToken(string token);
    }
}