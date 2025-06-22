using Domain.Users;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.PasswordHasher;
using Infrasctructure.UnitOfWork;
using Infrastrcture.Email;
using Infrasctructure.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Application.Users.Commands;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string Name,
    string Address,
    Role Role,
    string ReturnUrl,
    IFormFile? ImageFile = null
) : IRequest<Result>;

internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IEmailSenderService emailSender,
    IJwtService jwtService,
    IConfiguration configuration) : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userIsExist = await _userRepository.IsEmailExistAsync(request.Email, cancellationToken);
        if (userIsExist)
        {
            return Result.Failure("Пользователь с таким email уже существует.");
        }

        var hashPassword = _passwordHasher.HashPassword(request.Password);
        var confirmationToken = Guid.NewGuid().ToString();
        var refreshToken = _jwtService.GenerateRefreshToken();

        var user = new User(
            request.Name,
            request.Email,
            hashPassword,
            request.Address,
            isEmailConfirmed: false,
            confirmationToken,
            request.Role,
            imageUrl: null) 
        {
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenTTLDays"))
        };

        var encodedToken = WebUtility.UrlEncode(confirmationToken);
        var confirmationLink = $"{GetBaseUrl()}/api/user/confirm-email/{encodedToken}?returnUrl=" +
                               $"{WebUtility.UrlEncode(request.ReturnUrl)}";

        await _emailSender.SendEmailAsync(user.Email, "Подтвердите ваш email",
            $"Для подтверждения регистрации перейдите по <a href='{confirmationLink}'>ссылке</a>.");

        await _userRepository.AddAsync(user);
        await _unitOfWork.Commit();

        return Result.Success();
    }

    private static string GetBaseUrl() => "http://localhost:5000";
}
