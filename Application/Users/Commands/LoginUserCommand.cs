using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.Jwt;
using Infrasctructure.PasswordHasher;
using Microsoft.Extensions.Configuration;
using Infrasctructure.UnitOfWork;

namespace Application.Users.Commands;

public sealed record LoginUserCommand(string Email, string Password)
    : IRequest<Result<LoginUserResponse>>;

public sealed record LoginUserResponse(string Token, string RefreshToken);

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IPasswordHasher passwordHasher,
    IConfiguration configuration,
    IUnitOfWork unitOfWork
) : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
  
        var isExist = await _userRepository.IsEmailExistAsync(request.Email, cancellationToken);
        if (!isExist)
        {
            return Result.Failure<LoginUserResponse>("Пользователь с таким email не найден");
        }

        
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result.Failure<LoginUserResponse>("Пользователь с таким email не найден");
        }

       
        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.HashedPassword);
        if (!isPasswordValid)
        {
            return Result.Failure<LoginUserResponse>("Неверный пароль");
        }


        var loginResult = user.Login(isPasswordValid);
        if (loginResult.IsFailure)
        {
            return Result.Failure<LoginUserResponse>(loginResult.Error!);
        }

       
        var refreshToken = _jwtService.GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddMinutes(_configuration.
            GetValue<int>("Jwt:RefreshTokenExpirationTimeMinutes"));


        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiry;
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.Commit();


        var token = await _jwtService.GetToken(user.Id, user.Name, user.Role.ToString(), user.Email);

        return Result.Success(new LoginUserResponse(token, refreshToken));
    }
}
