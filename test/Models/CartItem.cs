using Test.Models.Core;

namespace Test.Models;

public class CartItem : EntityBase
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; }

    public CartItem()
    {
    }

    public CartItem(int id, int quantity, int productId, Product product, int cartId, Cart cart) : base(id)
    {
        Id = id;
        Quantity = quantity;
        ProductId = productId;
        Product = product;
        CartId = cartId;
        Cart = cart;
    }
}
