using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MediatR;
using Application.Carts.Queries;
using Application.Carts.Commands;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace Presentation.Carts;

public sealed class Cart : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cart = app.MapGroup("api/cart")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.User, SD.Role.Admin));

        cart.MapGet("/", GetCart);
        cart.MapPost("/", ClearCart);
    }

    //private static bool TryGetUserId(HttpContext httpContext, out int userId)
    //{
    //    var userIdClaim = httpContext.User.Claims
    //        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier
    //        || c.Type == JwtRegisteredClaimNames.Sub)?.Value;
    //    userId = 0;
    //    return userIdClaim is not null && int.TryParse(userIdClaim, out userId);
    //}

    private static async Task<IResult> GetCart(HttpContext context, [FromServices] ISender sender)
    {

        var response = await sender.Send(new GetCartQuery());

        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> ClearCart(HttpContext context, ISender sender)
    {

        var response = await sender.Send(new ClearCartCommand());
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

}
