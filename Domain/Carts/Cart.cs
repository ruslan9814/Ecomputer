using Domain.Users;
using System.Text.Json.Serialization;

namespace Domain.Carts;

public class Cart : EntityBase
{
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    public decimal TotalPrice => Items?.Sum(x => (x.Product?.Price ?? 0) * x.Quantity) ?? 0;

    private Cart() { }

    public Cart(int id, User user, ICollection<CartItem> cartItems) : base(id)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        UserId = user.Id;
        Items = cartItems ?? new List<CartItem>();
    }

    public Cart(User user) : base(0)
    {
        User = user;
        UserId = user.Id;
    }

    public Cart(int id, User user) : base(id)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        UserId = user.Id;
        Items = new List<CartItem>();
    }

    public Cart(int id, int userId) : base(id)
    {
        UserId = userId;
        Items = new List<CartItem>();
    }
}
