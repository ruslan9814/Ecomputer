using test.Common;
using test.Database.Service.UnitOfWork;
using Test.Database.Repositories.Interfaces;

public sealed record UpdateProfileCommand(int UserId, string Name, string Address, string PhoneNumber) : IRequest<Result>;


public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure("Пользователь не найден.");
        }

        user.Name = request.Name;
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.Commit();

        return Result.Success;
    }
}
