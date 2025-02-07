using Carter;
using Microsoft.AspNetCore.Mvc;
using test.Endpoints.Products.Requests;
using Test.Services.Product;
using test.CQRS.Products.Commands;
using test.CQRS.Products.Queries;

namespace test.Endpoints.Products;

////realizovat repositoriy cart and cartItem METODI DLA KORZINI ADD DELETE UPDATE NAPISAT V CLASSE cartItemRepository,
//// TAKJE DOVAVIT cartItemEndPoints i v ney vizvat metodi cerez ICartItemRepository

public sealed class ProductEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var product = app.MapGroup("api/products").RequireAuthorization(policy => policy.RequireRole("Admin"));
        product.MapGet("{productId}/", GetProduct);
        product.MapPost("/", AddProduct);
        product.MapDelete("{productId}/", RemoveProduct);
        product.MapPut("{productId}/", UpdateProduct);
    }

    public async Task<IResult> GetProduct([FromBody] GetProductRequest request, [FromServices] ISender sender)
    {
        var response = await sender.Send(new GetProductById(request.Id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> AddProduct([FromBody] AddProductRequest request, [FromServices] ISender sender)
    {
        var response = await sender.Send(new AddProduct(request.Name, 
            request.Description, request.Price, request.IsInStock));

        return response.IsFailure ?
            Results.BadRequest(response.Error) 
            : Results.Ok(response);
    }

    public async Task<IResult> RemoveProduct([FromBody] RemoveProductRequest request, [FromServices] ISender sender)
    {
         var response = await sender.Send(new DeleteProduct(request.Id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> UpdateProduct([FromBody] UpdateProductRequest request, [FromServices] ISender sender)
    {
       var response = await sender.Send(new UpdateProduct(request.Id, request.Name,
            request.Description, request.Price, request.Quantity, request.IsInStock));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }
}
