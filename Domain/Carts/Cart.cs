using Domain.Users;

namespace Domain.Carts;
public class Cart : EntityBase
{
    public ICollection<CartItem> Items { get; set; } = [];
    public int UserId { get; set; }
    public User User { get; set; }
    public decimal TotalPrice => Items.Sum(x => x.Product.Price * x.Quantity);

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Cart() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    public Cart(int id, User user, ICollection<CartItem> cartItems) : base(id)
    {
        User = user;
        Items = cartItems;
    }

    public Cart(int id, User user) : base(id)
    {
        User = user;
    }

}
