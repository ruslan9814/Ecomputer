using Domain.Users;
using System.Text.Json.Serialization;

namespace Domain.Carts;
public class Cart : EntityBase
{
    public ICollection<CartItem> Items { get; set; } = [];
    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
    public decimal TotalPrice => Items.Sum(x => x.Product.Price * x.Quantity);

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Cart() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    public Cart(User user)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        UserId = user.Id;
    }

    public Cart(int id, User user, ICollection<CartItem> cartItems) : base(id)
    {
        User = user;
        Items = cartItems;
    }

    public Cart(int id, User user) : base(id)
    {
        User = user;
    }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public Cart(int id) : base(id)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    {
        UserId = id;
        Items = [];
    }
}
