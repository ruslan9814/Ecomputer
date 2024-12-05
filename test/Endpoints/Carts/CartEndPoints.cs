using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Database.Repositories.Interfaces;
using test.Endpoints.CartItems.Models;
using test.Endpoints.Carts.Models;
using test.Models;

namespace test.Endpoints.Carts;

public sealed class CartEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cart = app.MapGroup("api/carts");
        //cart.MapGet("/{cartId}", CartExistsAsync);
        cart.MapGet("/{cartId}", GetCart);
    }

    public async Task<IResult> GetCart(int cartId, [FromServices] ICartRepository cartRepository)
    {
        var cart = await cartRepository.GetAsync(cartId);

        if (cart == null)
        {
            return Results.NotFound(new { Message = "Cart not found" });
        }

        if (cart.User == null)
        {
            return Results.NotFound(new { Message = "User not found for the cart" });
        }
        cart.Products ??= [];

        return Results.Ok(new CartResponse
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Products.Select(item => new CartItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                Quantity = item.Quantity,
                Price = item.Product.Price,
            }).ToList(),
            TotalSum = cart.TotalSum
        });
    }



    public async Task<IResult> CartExistsAsync(int cartId, [FromServices] ICartRepository cartRepository)
    {
        var exist = await cartRepository.CartExistsAsync(cartId);

        return Results.Ok(exist);
    }
}
