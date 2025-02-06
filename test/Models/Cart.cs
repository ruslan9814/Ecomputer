using Test.Models.Core;
using Test.Models;

public class Cart : EntityBase
{
    public ICollection<CartItem> Items { get; set; } = [];
    public int UserId { get; set; }
    public User User { get; set; }
    public decimal TotalPrice => Items.Sum(x => x.Product.Price * x.Quantity);

    private Cart()
    {
    }

    public Cart(int id, int userId, User user, ICollection<CartItem> cartItems) : base(id)
    {
        UserId = userId;
        User = user;
        Items = cartItems;
    }

    public Cart(int id, int userId, User user) : base(id)
    {
        UserId = userId;
        User = user;
    }

}
