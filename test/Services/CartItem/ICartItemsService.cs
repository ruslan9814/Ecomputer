using Microsoft.AspNetCore.Mvc;
using Test.Database.Repositories.Interfaces;
using Test.Endpoints.CartItems.Requests;
using Test.Endpoints.Carts.Responses;
using Test.Models;

namespace Test.Services.CartItemsService;

public interface ICartItemsService
{
    Task<bool> AddProduct(AddCartItemRequest cartItemRequest);
    Task<Product> UpdateQuantityProduct(UpdateCartItemRequest updateCartItemRequest);
    Task<CartResponse> DecreaseProductQuantity(RemoveCartItemRequest removeCartItemRequest);
}
