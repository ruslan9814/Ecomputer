using test.Endpoints.CartItems.Models;
using test.Models;

namespace test.Endpoints.Carts.Models;

public class CartResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItemResponse> Items { get; set; } = []; 
    public decimal TotalSum { get; set; }
}
