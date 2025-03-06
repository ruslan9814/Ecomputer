using Microsoft.AspNetCore.Mvc;
using Application.CartItems.Commands;
using Application.CartItems.Queries;
using Presentation.CartItems.Requests;
using Microsoft.AspNetCore.Http;
using MediatR;
using Api;

namespace Presentation.CartItems;

public sealed class CartItemEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cartItem = app.MapGroup("api/cart-item")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.UserAndAdmin));

        cartItem.MapPost("/", AddProductInCartItem);
        cartItem.MapPut("/", UpdateQuantity);
        cartItem.MapDelete("/", RemoveCartItem);
        cartItem.MapGet("/{Id}", GetCartItem);
        //cartItem.MapGet("/{cartId}", FindProductAsync);
        //cartItem.MapGet("/", GetProductAsync);
    }

    public async Task<IResult> GetCartItem(int id, ISender sender)
    {
        var response = await sender.Send(new GetByIdCartItemQuery(id));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }

    public async Task<IResult> AddProductInCartItem([FromBody] AddCartItemRequest request, ISender sender)
    {
        var response = await sender.Send(new AddCartItemCommand(request.CartId,
            request.ProductId, request.Quantity));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }



    public async Task<IResult> UpdateQuantity([FromBody] UpdateCartItemRequest request, ISender sender)
    {
        var response = await sender.Send(new UpdateQuantityCartItemCommand(request.Id, request.Quantity));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }

    public async Task<IResult> RemoveCartItem([FromBody] RemoveCartItemRequest request, ISender sender)
    {
        var response = await sender.Send(new RemoveCartItemCommand(request.CartItemId));

        return response.IsFailure ?
             Results.BadRequest(response.Error) : Results.Ok(response);
    }
}
