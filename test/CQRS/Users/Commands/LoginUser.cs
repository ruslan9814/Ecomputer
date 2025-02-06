using test.Common;
using Test.Database.Repositories.Interfaces;

public sealed record LoginUser(string Email, string Password) : IRequest<Result>;

public sealed class LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : 
    IRequestHandler<LoginUser, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            return Result.Failure("Пользователь с таким email не найден");
        }

        var isHashedPassword = _passwordHasher.VerifyPassword(request.Password, user.HashedPassword);

        if (!isHashedPassword) 
        {
            return Result.Failure("Неверный пароль.");
        }

        return Result.Success;

    }
}
