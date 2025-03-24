using Domain.Carts;
using Domain.Favorites;
using Domain.Orders;

namespace Domain.Users;

public class User : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Address { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsBlocked { get; set; }
    public string? ConfirmationToken { get; set; }
    public string RefreshToken { get; set; }
    public Cart? Cart { get; set; }
    public Role Role { get; set; }
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Favorite> Favorites { get; set; } = [];
    public DateTime RefreshTokenExpiryTime { get; set; }


#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private User() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    public User(string name, string email, string hashedPassword, string address, bool isEmailConfirmed, string? confirmationToken, Role role)
    {
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
        Address = address;
        IsEmailConfirmed = isEmailConfirmed;
        ConfirmationToken = confirmationToken;
        RefreshToken = null!;
        Role = role;
        Cart = new Cart(this);
    }

    public Result Login(bool isVerify)
    {
        if (!isVerify)
        {
            return Result.Failure("Пользователь с таким email не найден.");
        }

        if (!IsEmailConfirmed)
        {
            return Result.Failure("Email не подтвержден. Пожалуйста, подтвердите свой email.");
        }

        return Result.Success();
    }

    public Result ConfirmEmail()
    {
        if (IsEmailConfirmed)
        {
            return Result.Failure("Email уже подтвержден.");
        }

        IsEmailConfirmed = true;
        ConfirmationToken = null;

        return Result.Success();
    }
}