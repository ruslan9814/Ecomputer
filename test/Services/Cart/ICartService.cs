using Test.Endpoints.Carts.Responses;

namespace Test.Services.Cart;

public interface ICartService
{
    Task<CartResponse?> GetCart(int cartId);
}
