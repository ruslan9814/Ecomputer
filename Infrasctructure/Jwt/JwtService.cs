using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrasctructure.Jwt;

public class JwtService(IOptions<JwtOptions> options, IDistributedCache database) : IJwtService
{
    private readonly JwtOptions _options = options.Value;
    private readonly IDistributedCache _redisDatabase = database;

    public async Task<string> GetToken(int userId, string username, string role, string email)
    {

        var claims = CreateClaims(userId, username, role, email);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.TokenExpirationTimeMinutes),
            signingCredentials: signingCredentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);


        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.TokenExpirationTimeMinutes)
        };

        await _redisDatabase.SetStringAsync($"access_token:{userId}", tokenValue, options);

        return tokenValue;
    }

    private static Claim[] CreateClaims(int userId, string username, string role, string email) =>
        [
        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, username),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        ];

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes)
       .Replace('+', '-') 
       .Replace('/', '_')
       .TrimEnd('=');

        return token;
    }

    public int GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        return userId is null ? 
            throw new Exception() 
            : int.Parse(userId);
    }

}

