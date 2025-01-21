using Carter;
using Microsoft.AspNetCore.Mvc;
using Test.Services.Cart;

namespace Test.Endpoints.Carts;

public sealed class CartEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cart = app.MapGroup("api/carts").RequireAuthorization(policy => policy.RequireRole(SD.Role.UserAndAdmin));

        cart.MapGet("{cartId}", GetCart);
    }

    private static async Task<IResult> GetCart([FromRoute] int cartId, [FromServices] CartService cartService)
    {
        var cart = await cartService.GetCart(cartId);
        return cart is null ? Results.NotFound(new { Message = "Cart not found" }) 
            : Results.Ok(cart);
    }

}
