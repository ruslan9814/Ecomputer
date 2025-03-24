using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Users.Commands;
public sealed record DeleteUserByIdCommand(int Id) : IRequest<Result>;

internal sealed class DeleteUserByIdCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByIdCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var userIsExist = await _userRepository.IsExistAsync(request.Id);

        if (!userIsExist)
        {
            return Result.Failure("Пользователь не найден.");
        }

        await _userRepository.DeleteAsync(request.Id);
        await _unitOfWork.Commit();
        return Result.Success();
    }
}