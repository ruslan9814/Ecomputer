using Carter;
using Microsoft.AspNetCore.Mvc;
using test.CQRS.Carts.Queries;
using Test.Services.Cart;

namespace Test.Endpoints.Carts;

public sealed class CartEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cart = app.MapGroup("api/carts").RequireAuthorization(policy => policy.RequireRole(SD.Role.UserAndAdmin));

        cart.MapGet("{cartId}", GetCart);
    }

    private static async Task<IResult> GetCart(int cartId, [FromServices] ISender sender)
    {
        var response = await sender.Send(new GetByUserIdCart(cartId));

        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

}
