using Domain.Users;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.PasswordHasher;
using Infrasctructure.UnitOfWork;
using Infrastrcture.Email;

namespace Application.Users.Commands;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string Name,
    string Address,
    Role Role, 
    string ReturnUrl) : IRequest<Result>;

public sealed class RegisterUserCommandHandler(IUserRepository userRepository, 
    IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, IEmailSenderService emailSender) 
    : IRequestHandler<RegisterUserCommand, Result>
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailSenderService _emailSender = emailSender;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userIsExist = await _userRepository.IsEmailExistAsync(request.Email);

        if (userIsExist)
        {
            return Result.Failure("Пользователь с таким email уже существует.");
        }

        var hashPassword = _passwordHasher.HashPassword(request.Password);
        var confirmationToken = Guid.NewGuid().ToString();

        var user = new User(
            request.Name,
            request.Email,
            hashPassword,
            request.Address,
            isEmailConfirmed: false,
            confirmationToken,
            request.Role);
        

        var confirmationLink = $"http://localhost:5000/api/user/confirm-email/{confirmationToken}?returnUrl={request.ReturnUrl}";

        await _emailSender.SendEmailAsync(user.Email, "Подтвердите ваш email",
            $"Для подтверждения регистрации перейдите по <a href='{confirmationLink}'>ссылке</a>.");

        await _userRepository.AddAsync(user);
        await _unitOfWork.Commit();

        return Result.Success();
    }
}



