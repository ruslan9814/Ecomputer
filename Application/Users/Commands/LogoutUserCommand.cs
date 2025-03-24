using Infrasctructure.BlackList;
using Infrasctructure.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Users.Commands;

public sealed record LogoutUserCommand(string Token) : IRequest<Result>;

internal sealed class LogoutCommandHandler(
    IUserRepository userRepository,
    IBlackListService blackList,
    IDistributedCache redisCache,
    IConfiguration configuration) : IRequestHandler<LogoutUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IBlackListService _blackList = blackList;
    private readonly IDistributedCache _redisCache = redisCache;
    private readonly IConfiguration _configuration = configuration;

    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        if (!tokenHandler.CanReadToken(request.Token))
        {
            return Result.Failure("Неверный формат токена");
        }

        var jwtToken = tokenHandler.ReadJwtToken(request.Token);
        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure("Токен не содержит идентификатор пользователя");
        }


        var user = await _userRepository.GetAsync(int.Parse(userId));
        if (user is null)
        {
            return Result.Failure("Пользователь не найден");
        }

        var tokenTtl = TimeSpan.FromMinutes(
            _configuration.GetValue<int>("Jwt:TokenExpirationTimeMinutes"));
        await _blackList.AddTokenToBlackList(request.Token, tokenTtl);


        await _redisCache.RemoveAsync($"access_token:{userId}", cancellationToken);

        user.RefreshToken = null!;
        user.RefreshTokenExpiryTime = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();

    }
}

