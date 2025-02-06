using Test.Models.Core;
using Test.Models;

public class Cart : EntityBase
{
    public decimal TotalSum { get; set; }
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    public int UserId { get; set; }
    public User User { get; set; }

    public decimal TotalPrice => Items.Sum(x => x.Product.Price * x.Quantity);

    private Cart()
    {
    }

    public Cart(int id, decimal totalSum, int userId, User user, ICollection<CartItem> products) : base(id)
    {
        UserId = userId;
        User = user;
        Items = products;
        TotalSum = products.Sum(x => x.Product.Price * x.Quantity);
    }
}
