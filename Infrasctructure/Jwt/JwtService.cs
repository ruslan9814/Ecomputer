using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrasctructure.Jwt;

public class JwtService(IOptions<JwtOptions> options, IDistributedCache database) : IJwtService
{
    private readonly JwtOptions _options = options.Value;
    private readonly IDistributedCache _redisDatabase = database;

    public async Task<string> GetToken(string username, string role, string email)
    {

        var claims = CreateClaims(username, role, email);

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

        await _redisDatabase.SetStringAsync(username, tokenValue, options);

        return tokenValue;
    }

    private static Claim[] CreateClaims(string username, string role, string email) =>
        [
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Email, email)
        ];

    public string GetRefreshToken()
    {
        var random = Guid.NewGuid().ToByteArray();

        return Convert.ToBase64String(random);
    }
}

