using test.Common;
using test.Database.Service.PasswordHasher;
using test.Database.Service.UnitOfWork;
using Test.Database.Repositories.Interfaces;
using Test.Models;

public sealed record ChangePasswordCommand(int UserId, string OldPassword, string NewPassword) : IRequest<Result>;


public sealed class ChangePasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.UserId);

        if (user is null)
        {
            return  Result.Failure("Пользователь не найден.");
        }

        var verificationResult =  _passwordHasher.VerifyPassword(user.HashedPassword, request.OldPassword);

        if (!verificationResult)
        {
            return Result.Failure("Неверный старый пароль.");
        }

        var newHashedPassword = _passwordHasher.HashPassword(request.NewPassword);

        user.HashedPassword = newHashedPassword;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}