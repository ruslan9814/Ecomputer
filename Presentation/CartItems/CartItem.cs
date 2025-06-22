using Microsoft.AspNetCore.Mvc;
using Application.CartItems.Commands;
using Application.CartItems.Queries;
using Presentation.CartItems.Requests;
using Microsoft.AspNetCore.Http;
using MediatR;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Presentation.CartItems;

public sealed class CartItem : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cartItem = app.MapGroup("api/cart-item")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin, SD.Role.User));

        cartItem.MapPost("/", AddProductInCartItem);
        cartItem.MapPut("/", UpdateQuantity);
        cartItem.MapDelete("/{Id}", RemoveCartItem);
        cartItem.MapGet("/{id}", GetCartItem);
    }

    private static async Task<IResult> GetCartItem(int id, ISender sender)
    {
        var response = await sender.Send(new GetByIdCartItemQuery(id));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> AddProductInCartItem([FromBody] AddCartItemRequest request, 
        ISender sender)
    {
        var response = await sender.Send(new AddCartItemCommand(request.CartId,
            request.ProductId, request.Quantity));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> UpdateQuantity([FromBody] UpdateCartItemRequest request, 
        ISender sender)
    {
        var response = await sender.Send(new UpdateQuantityCartItemCommand(request.Id, 
            request.Quantity));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> RemoveCartItem(int id, ISender sender)
    {
        var response = await sender.Send(new RemoveCartItemCommand(id));

        return response.IsFailure ?
             Results.BadRequest(response.Error) : Results.Ok(response);
    }
}
