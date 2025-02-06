using test.Common;
using test.Database.DbService;
using Test.Database.Repositories.Interfaces;

public sealed record UpdateProfile(int UserId, string Name, string Address, string PhoneNumber) : IRequest<Result>;


public sealed class UpdateProfileHandler : IRequestHandler<UpdateProfile, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result> Handle(UpdateProfile request, CancellationToken cancellationToken)
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
