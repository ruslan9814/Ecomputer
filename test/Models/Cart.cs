using Test.Models.Core;

namespace Test.Models;

public class Cart : EntityBase
{
    public decimal TotalSum { get; set; }
    public ICollection<CartItem> Items { get; set; } = [];
    public int UserId { get; set; }
    public User User { get; set; }

    public decimal TotalPrice => Items.Sum(x => x.Product.Price * x.Quantity);

    private Cart()
    {
    }

    public Cart(int id, decimal totalSum, int userId, User user, ICollection<CartItem> products) : base(id)
    {
        TotalSum = totalSum;
        UserId = userId;
        User = user;
        Items = products;
    }
}