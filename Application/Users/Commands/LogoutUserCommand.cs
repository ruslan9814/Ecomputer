using Infrasctructure.BlackList;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Users.Commands;

public sealed record LogoutUserCommand(string Token) : IRequest<Result>;

internal sealed class LogoutCommandHandler(IUserRepository userRepository, IBlackListService blackList) : 
    IRequestHandler<LogoutUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IBlackListService _blackList = blackList;

    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByConfirmationTokenAsync(request.Token);
        if (user is null)
        {
            return Result.Failure("Пользователь не найден");
        }
        var ttl = TimeSpan.FromMinutes(30);
        await _blackList.AddTokenToBlackList(request.Token, ttl);
        user.RefreshToken = null!;
        user.RefreshTokenExpirationDate = DateTime.Now;
        await _userRepository.UpdateAsync(user);
        return Result.Success();
    }
}

