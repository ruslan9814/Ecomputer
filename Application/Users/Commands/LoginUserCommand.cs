using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.Jwt;
using Infrasctructure.PasswordHasher;

namespace Application.Users.Commands;
public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<string>>;

internal sealed class LoginUserCommandHandler(IUserRepository userRepository, IJwtService jwtService,
    IPasswordHasher passwordHasher) : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtService _jwtService = jwtService;

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var isExist = await _userRepository.IsEmailExistAsync(request.Email);

        if (!isExist)
        {
            return Result.Failure<string>("Пользователь с таким email не найден");
        }

        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        var isHashedPassword = _passwordHasher.VerifyPassword(request.Password, user.HashedPassword);

        var loginResult = user.Login(isHashedPassword);

        if (loginResult.IsFailure)
        {
            return Result.Failure<string>(loginResult.Error!);
        }

        var token = await _jwtService.GetToken(user.Name, user.Role.ToString(), user.Email);

        var refreshToken = _jwtService.GetRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpirationDate = DateTime.UtcNow.AddMinutes(30);
        await _userRepository.UpdateAsync(user);

        return Result.Success(token);
    }
}