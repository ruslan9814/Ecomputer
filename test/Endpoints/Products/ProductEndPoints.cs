using Carter;
using Microsoft.AspNetCore.Mvc;
using test.Endpoints.Products.Requests;
using Test.Services.Product;

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

    public async Task<IResult> GetProduct([FromBody] GetProductRequest request, [FromServices] IProductService productService)
    {
        var product = await productService.GetProductAsync(request);
        return product is null
            ? Results.NotFound(new { Message = "Product not found" })
            : Results.Ok(product);
    }

    private async Task<IResult> AddProduct([FromBody] AddProductRequest request, [FromServices] IProductService productService)
    {
        var product = await productService.AddProductAsync(request);
        return product is null
            ? Results.BadRequest(new { Message = "Invalid product data" })
            : Results.Created($"/api/products/{product.Id}", product);
    }

    public async Task<IResult> RemoveProduct([FromBody] RemoveProductRequest request, [FromServices] IProductService productService)
    {
        var success = await productService.RemoveProductAsync(request);
        return success
            ? Results.Ok(new { Message = "Product removed successfully" })
            : Results.NotFound(new { Message = "Product not found" });
    }

    private async Task<IResult> UpdateProduct([FromBody] UpdateProductRequest request, [FromServices] IProductService productService)
    {
        var product = await productService.UpdateProductAsync(request);
        return product is null
            ? Results.NotFound(new { Message = "Product not found or invalid request" })
            : Results.Ok(product);
    }
}