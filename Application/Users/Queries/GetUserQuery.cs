using Application.Dtos;
using Infrasctructure.Repositories.Interfaces;

namespace Application.Users.Queries;

public sealed record GetUserQuery(int Id) : IRequest<Result<UserDto>>;

internal sealed class GetUserQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
    
        if (!await _userRepository.IsExistAsync(request.Id))
        {
            return Result.Failure<UserDto>($"Пользователь с ID {request.Id} не найден.");
        }

        var user = await _userRepository.GetAsync(request.Id);
       
        var response = new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Address,
            user.ImageUrl
        );

        return Result.Success(response);
    }
}
