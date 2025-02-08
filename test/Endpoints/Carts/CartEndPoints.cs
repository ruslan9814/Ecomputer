using Carter;
using Microsoft.AspNetCore.Mvc;
using test.CQRS.Carts.Queries;

namespace Test.Endpoints.Carts;

public sealed class CartEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cart = app.MapGroup("api/cart").RequireAuthorization(policy => policy.RequireRole(SD.Role.UserAndAdmin));

        cart.MapGet("{Id}", GetCart);
        cart.MapPost("/", ClearCart);
    }

    private static async Task<IResult> GetCart(int Id, [FromServices] ISender sender)
    {
        var response = await sender.Send(new GetByUserIdCartQuery(Id));

        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    public static async Task<IResult> ClearCart(int Id, ISender sender)
    {
        var response = await sender.Send(new ClearCartCommand(Id));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

}
