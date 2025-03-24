using Infrasctructure.Jwt;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;

namespace Application.Users.Commands;

public sealed record UpdateRefreshTokenCommand(string Token, string RefreshToken) 
    : IRequest<Result<UpdateRefreshTokenCommandResponse>>;
public sealed record UpdateRefreshTokenCommandResponse(string Token, string RefreshToken);
internal sealed class UpdateRefreshTokenHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IUnitOfWork unitOfWork) :
    IRequestHandler<UpdateRefreshTokenCommand, Result<UpdateRefreshTokenCommandResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<UpdateRefreshTokenCommandResponse>> Handle(
        UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _jwtService.GetUserIdFromToken(request.Token);

        var user = await _userRepository.GetAsync(userId);

        if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return Result
                .Failure<UpdateRefreshTokenCommandResponse>("Токен истек");
        }

        if (user.RefreshToken != request.RefreshToken)
        {
            return Result
                .Failure<UpdateRefreshTokenCommandResponse>("Токены не совпадают");
        }

        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        var token = await _jwtService.GetToken(user.Id, user.Name, user.Role.ToString(), user.Email);

        user.RefreshTokenExpiryTime = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.Commit();

        return Result.Success(new UpdateRefreshTokenCommandResponse(token, refreshToken));
    }
}
