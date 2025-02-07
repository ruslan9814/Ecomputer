using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;
using Test.Models;

public sealed record RegisterUser(string Email, string Password, string Name, string Address) : IRequest<Result>;

public sealed class RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork) : IRequestHandler<RegisterUser, Result>
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var userIsExist = await _userRepository.IsEmailExistAsync(request.Email);

        if (userIsExist)
        {
            return Result.Failure("Пользователь с таким email уже существует.");
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Address = request.Address,
            HashedPassword = _passwordHasher.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}


public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}


