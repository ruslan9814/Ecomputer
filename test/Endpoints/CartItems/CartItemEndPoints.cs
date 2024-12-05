using Carter;
using Microsoft.AspNetCore.Mvc;
using test.Database;
using test.Database.Repositories.Interfaces;
using test.Endpoints.CartItems.Models;
using test.Endpoints.Carts.Models;
using test.Models;

namespace test.Endpoints;

public sealed class CartItemEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var cartItem = app.MapGroup("api/carts");

        cartItem.MapPost("/", AddProduct);
        //cartItem.MapDelete("/", RemoveProductAsync);
        //cartItem.MapGet("/{cartId}", FindProductAsync);
        //cartItem.MapGet("/", GetProductAsync);
    }

    public async Task<IResult> AddProduct(int cartId, [FromBody] CartItemRequest cartItemRequest, [FromServices] ICartRepository cartRepository, [FromServices] IProductRepository productRepository)
    {
        var cart = await cartRepository.GetAsync(cartId);
        if (cart == null)
        {
            return Results.NotFound(new { Message = "Cart not found" });
        }

        var product = await productRepository.GetAsync(cartItemRequest.ProductId);
        if (product == null)
        {
            return Results.NotFound(new { Message = "Product not found" });
        }

        cart.Products ??= [];

        var cartItem = new CartItem
        {
            CartId = cartId,
            ProductId = cartItemRequest.ProductId,
            Quantity = cartItemRequest.Quantity,
            Product = product
        };

        cart.Products.Add(cartItem);
        cart.TotalSum += product.Price * cartItem.Quantity;

        await cartRepository.UpdateAsync(cart);

      
        var cartResponse = new CartResponse
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Products.Select(item => new CartItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                Price = item.Product.Price
            }).ToList(),
            TotalSum = cart.TotalSum
        };

        return Results.Ok(cartResponse);
    }



}
