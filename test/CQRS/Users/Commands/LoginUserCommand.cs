using test.Common;
using test.Database.Service.PasswordHasher;
using Test.Database.Repositories.Interfaces;
using Test.Infrastrcture.Jwt;

public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<string>>;

public sealed class LoginUserCommandHandler(IUserRepository userRepository, JwtService jwtService,
    IPasswordHasher passwordHasher) : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly JwtService jwtService = jwtService;

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var isExist = await _userRepository.IsEmailExistAsync(request.Email);

        if (!isExist)
        {
            return Result<string>.Failure("Пользователь с таким email не найден");
        }

        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        var isHashedPassword = _passwordHasher.VerifyPassword(request.Password, user.HashedPassword);

        var loginResult = user.Login(isHashedPassword);

        if (loginResult.IsFailure) 
        {
            return Result<string>.Failure(loginResult.Error!);
        }

        var token = jwtService.GetToken(user.Name, user.Role.ToString());

        return Result<string>.Success(token);

    }
}
