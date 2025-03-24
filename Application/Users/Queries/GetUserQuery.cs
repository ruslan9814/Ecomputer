using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Users.Queries;

public sealed record GetUserQuery(int Id) : IRequest<Result<UserDto>>;

internal sealed class GetUserQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<Result<UserDto>> Handle(GetUserQuery request, 
        CancellationToken cancellationToken)
    {
        var userIsExists = await _userRepository.IsExistAsync(request.Id);
        if (!userIsExists)
        {
            return Result.Failure<UserDto>($"Продукт с ID {request.Id} не найден.");
        }

        var user = await _userRepository.GetAsync(request.Id);

        var response = new UserDto(
            user.Id,
            user.Email,
            user.Name
        );
        return Result.Success(response);
    }
}
