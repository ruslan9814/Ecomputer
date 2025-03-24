using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.PasswordHasher;
using Infrasctructure.UnitOfWork;

namespace Application.Users.Commands;
public sealed record ChangePasswordCommand(
    int UserId, 
    string OldPassword, 
    string NewPassword) : IRequest<Result>;


internal sealed class ChangePasswordCommandHandler(
    IUserRepository userRepository, 
    IPasswordHasher passwordHasher, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var isExist = await _userRepository.IsExistAsync(request.UserId);

        if (!isExist)
        {
            return Result.Failure("Пользователь не найден.");
        }

        var user = await _userRepository.GetAsync(request.UserId);

        var verificationResult = _passwordHasher.VerifyPassword(user.HashedPassword, request.OldPassword);

        if (!verificationResult)
        {
            return Result.Failure("Неверный старый пароль.");
        }

        var newHashedPassword = _passwordHasher.HashPassword(request.NewPassword);

        user.HashedPassword = newHashedPassword;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.Commit();
        return Result.Success();
    }
}