using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Test.Endpoints.CartItems.Requests;
using Test.Services.CartItemsService;

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

        return response is null ? Results.BadRequest("not found") : Results.Ok(response);
    }

    public async Task<IResult> AddProductInCartItem([FromBody] AddCartItemRequest addCartItemRequest, [FromServices] ICartItemsService cartItemsService)
    {
        var result = await cartItemsService.AddProduct(addCartItemRequest);

        if (!result)
        {
            return Results.Ok(new { Message = "cant added" });
        }

        return Results.Ok(result);
    }


    public async Task<IResult> UpdateQuantity([FromBody] UpdateCartItemRequest updateCartItemRequest, [FromServices] ICartItemsService cartItemsService)
    {
        var result = await cartItemsService.UpdateQuantityProduct(updateCartItemRequest);

        if (result is null)
        {
            return Results.NotFound(new { Message = "Product not found or invalid request" });
        }

        return Results.Ok(result);
    }

    public async Task<IResult> RemoveCartItem([FromBody] RemoveCartItemRequest removeCartItemRequest, [FromServices] ICartItemsService cartItemsService)
    {
        var result = await cartItemsService.DecreaseProductQuantity(removeCartItemRequest);

        if (result is null)
        {
            return Results.NotFound(new { Message = "Product not found or invalid request" });
        }

        return Results.Ok(result);
    }



}
