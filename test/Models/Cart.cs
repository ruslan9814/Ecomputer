namespace test.Models;

public class Cart
{
    public int Id { get; set; }
    public decimal TotalSum { get; set; }
    public ICollection<CartItem> Products { get; set; } = [];
    public int UserId { get; set; }
    public User User { get; set; }
}