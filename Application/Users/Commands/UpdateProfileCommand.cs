using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;
using Infrasctructure.PasswordHasher;

namespace Application.Users.Commands;

public sealed record UpdateProfileCommand(
    int UserId,
    string? Name,
    string? Address,
    string? Password
    ) : IRequest<Result>;

internal sealed class UpdateProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure("Пользователь не найден.");
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            user.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Address))
        {
            user.Address = request.Address;
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.HashedPassword = _passwordHasher.HashPassword(request.Password);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
