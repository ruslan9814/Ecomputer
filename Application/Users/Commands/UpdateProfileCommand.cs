using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Users.Commands;

public sealed record UpdateProfileCommand(int UserId, string Name, string Address) : IRequest<Result>;


public sealed class UpdateProfileCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure("Пользователь не найден.");
        }

        user.Name = request.Name;
        user.Address = request.Address;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}
