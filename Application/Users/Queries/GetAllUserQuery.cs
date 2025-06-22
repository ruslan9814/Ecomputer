using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Users.Queries;

public sealed record GetAllUserQuery() : IRequest<Result<IEnumerable<UserDto>>>;

internal sealed class GetAllUserQueryHandler(
    IUserRepository userRepository) : IRequestHandler<GetAllUserQuery, Result<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUserQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll(includeRelated: true);

        if (users == null || !users.Any())
        {
            return Result.Failure<IEnumerable<UserDto>>("Пользователи не найдены.");
        }

        var userDtos = users.Select(p => new UserDto(
            p.Id,
            p.Name,
            p.Email,
            p.Address,
            p.ImageUrl
        )).ToList();

        return Result.Success<IEnumerable<UserDto>>(userDtos);
    }
}