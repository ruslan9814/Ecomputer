using test.Services.CartItem.Responses;
using Test.Database.Repositories.Interfaces;


namespace Test.Services.Cart;

public class CartService(ICartRepository cartRepository)
{
    private readonly ICartRepository _cartRepository = cartRepository;

    public async Task<CartResponse?> GetCart(int cartId)
    {
        var isExist = await _cartRepository.IsExistAsync(cartId);

        if (!isExist)
        {
            return null;    
        }

        var cart = await _cartRepository.GetAsync(cartId);

        return new CartResponse
        (
            cart.Id,
            cart.UserId,
            cart.Items.Select(item => new CartItemResponse
            (
                item.Id,
                item.ProductId,
                item.Product.Name,
                item.Quantity,
                item.Product.Price
            )).ToList(),
            cart.TotalPrice
        );
    }

}
