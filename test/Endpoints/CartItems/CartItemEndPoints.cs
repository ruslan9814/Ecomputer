using Carter;
using Microsoft.AspNetCore.Mvc;
using test.CQRS.CartItems.Commands;
using test.CQRS.CartItems.Queries;
using Test.Endpoints.CartItems.Requests;


namespace Test.Endpoints.CartItems;

public sealed class CartItemEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cartItem = app.MapGroup("api/carts")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.UserAndAdmin));

        cartItem.MapPost("/", AddProductInCartItem);
        cartItem.MapPut("/", UpdateQuantity);
        cartItem.MapDelete("/", RemoveCartItem);
        cartItem.MapGet("/{id}", GetCartItem);
        //cartItem.MapGet("/{cartId}", FindProductAsync);
        //cartItem.MapGet("/", GetProductAsync);
    }

    public async Task<IResult> GetCartItem(int id, ISender sender)
    {
        var response = await sender.Send(new GetByIdCartItemQuery(id));

        return response.IsFailure ? 
            Results.BadRequest(response.Error) : Results.Ok(response);
    }

    public async Task<IResult> AddProductInCartItem([FromBody] AddCartItemRequest  addCartItemRequest, [FromServices] ISender sender)
    {
        var response = await sender.Send(new AddCartItem(addCartItemRequest.CartId, 
            addCartItemRequest.ProductId, addCartItemRequest.Quantity));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }



    public async Task<IResult> UpdateQuantity([FromBody] UpdateCartItemRequest updateCartItemRequest, [FromServices] ISender sender)
    {
        var response = await sender.Send(new UpdateQuantityCartItem(updateCartItemRequest.Id, updateCartItemRequest.Quantity));

        return response.IsFailure ?
            Results.BadRequest(response.Error) : Results.Ok(response);
    }

    public async Task<IResult> RemoveCartItem([FromBody] RemoveCartItemRequest removeCartItemRequest, [FromServices] ISender sender)
    {
        var response = await sender.Send(new RemoveCartItem(removeCartItemRequest.CartItemId));

        return response.IsFailure ?
             Results.BadRequest(response.Error) : Results.Ok(response);
    }
}
