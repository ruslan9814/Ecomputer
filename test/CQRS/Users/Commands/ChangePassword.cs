using Microsoft.AspNetCore.Identity;
using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;
using Test.Models;

public sealed record ChangePassword(int UserId, string OldPassword, string NewPassword) : IRequest<Result>;


public sealed class ChangePasswordHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork) : IRequestHandler<ChangePassword, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<User> _passwordHasher = (IPasswordHasher<User>)passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.UserId);

        if (user is null)
        {
            return  Result.Failure("Пользователь не найден.");
        }

        var verificationResult =  _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.OldPassword);

        if (verificationResult is PasswordVerificationResult.Failed)
        {
            return Result.Failure("Неверный старый пароль.");
        }

        var newHashedPassword = _passwordHasher.HashPassword(user, request.NewPassword);

        user.HashedPassword = newHashedPassword;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}