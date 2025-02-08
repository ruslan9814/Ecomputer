using Castle.Core.Smtp;
using test.Common;
using test.Database.Service.PasswordHasher;
using test.Database.Service.UnitOfWork;
using test.Infrastrcture.Email;
using Test.Database.Repositories.Interfaces;
using Test.Models;

public sealed record RegisterUserCommand(string Email, string Password, string Name, string Address) : IRequest<Result>;

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

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Address = request.Address,
            HashedPassword = _passwordHasher.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.Commit();

        await _emailSender.SendEmailAsync(user.Email, "Регистрация", "Вы успешно зарегистрировались");



        return Result.Success;
    }
}



