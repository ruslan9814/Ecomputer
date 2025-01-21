
using test.Services.CartItem.Responses;
using Test.Database.Repositories.Interfaces;
using Test.Endpoints.CartItems.Requests;
using Test.Models;
using Test.Services.CartItemsService;

namespace test.Services;

public class CartItemsService(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    ICartItemRepository cartItemRepository)
    : ICartItemsService
{
    private readonly ICartRepository _cartRepository = cartRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICartItemRepository _cartItemRepository = cartItemRepository;
    public async Task<bool> AddProduct(AddCartItemRequest request)
    {
        var isExist = await _cartRepository.IsExistAsync(request.CartId);

        if (!isExist)
        {
            return false;
        }

        //TODO: добавить логику затем добавиить метод для update quantity
        var cartItemIsExist = _cartItemRepository.IsExistAsync(request.CartId, request.ProductId);

        if (cartItemIsExist)
        {
            _cartItemRepository.;

            return true;
        }

        var productIsExist = await _productRepository.IsExistAsync(request.ProductId);
        if (!productIsExist)
        {
            return false;
        }


        var cartItem = new Test.Models.CartItem
        {
            CartId = request.CartId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
        };

        await _cartItemRepository.AddAsync(cartItem);
        return true;
    }

    public async Task<Product> UpdateQuantityProduct(UpdateCartItemRequest request)
    {

        if (request is null)
        {
            return null;
        }

        var product = await _productRepository.GetAsync(request.Id);
        if (product is null)
        {
            return null;
        }

        product.Quantity += request.Quantity;

        await _productRepository.UpdateAsync(product);

        return product;

    }





    public async Task<CartResponse> DecreaseProductQuantity(RemoveCartItemRequest removeCartItemRequest)
    {
        var cart = await _cartRepository.GetAsync(removeCartItemRequest.CartItemId);
        if (cart is null)
        {
            return null;
        }


        var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == removeCartItemRequest.CartItemId);
        if (cartItem is null || cartItem.Quantity <= removeCartItemRequest.CartItemId)
        {
            return null;
        }


        cartItem.Quantity -= removeCartItemRequest.CartItemId;
        cart.TotalSum -= cartItem.Product.Price * removeCartItemRequest.CartItemId;

        await _cartRepository.UpdateAsync(cart);

        return new CartResponse(
            cart.Id,
            cart.UserId,
            cart.Items.Select(item => new CartItemResponse(
                item.Id,
                item.ProductId,
                item.Product.Name,
                item.Quantity,
                item.Product.Price
            )).ToList(),
            cart.TotalSum
        );
    }

    Task<Product> ICartItemsService.UpdateQuantityProduct(UpdateCartItemRequest updateCartItemRequest)
    {
        throw new NotImplementedException();
    }

    Task<CartResponse> ICartItemsService.DecreaseProductQuantity(RemoveCartItemRequest removeCartItemRequest)
    {
        throw new NotImplementedException();
    }
}
