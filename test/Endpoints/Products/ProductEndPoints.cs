using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using test.Database;
using test.Database.Repositories.Interfaces;
using test.Endpoints.Products.Models;
using test.Endpoints.Users.Models;
using test.Models;

namespace test.Endpoints.Products;

////realizovat repositoriy cart and cartItem METODI DLA KORZINI ADD DELETE UPDATE NAPISAT V CLASSE cartItemRepository,
//// TAKJE DOVAVIT cartItemEndPoints i v ney vizvat metodi cerez ICartItemRepository

public sealed class ProductEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var product = app.MapGroup("api/produts");
        product.MapGet("{productId}/", GetProduct);
        product.MapPost("/", AddProduct);
        product.MapDelete("{productId}/", RemoveProduct);
        product.MapPut("{productId}/", UpdateProduct);
    }

    public async Task<IResult> GetProduct(int productId, [FromServices] IProductRepository productRepository)
    {
        var product = await productRepository.GetAsync(productId);

        if (product == null)
        {
            return Results.NotFound(new { Message = "Product not found" });
        }

        return Results.Ok(new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Quantity = product.Quantity,
            IsInStock = product.Quantity > 0
        });
    }


    public async Task<IResult> AddProduct([FromBody] ProductRequest productRequest, [FromServices] IProductRepository productRepository)
    {
        if (string.IsNullOrEmpty(productRequest.Name) || productRequest.Price <= 0 || productRequest.Quantity < 0)
        {
            return Results.BadRequest(new { Message = "Invalid product data" });
        }

        var product = new Product
        {
            Name = productRequest.Name,
            Price = productRequest.Price,
            Description = productRequest.Description,
            Quantity = productRequest.Quantity,
            IsInStock = productRequest.Quantity > 0
        };

        await productRepository.AddAsync(product);

        return Results.Ok(new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Quantity = product.Quantity,
            IsInStock = product.IsInStock
        });
    }

    public async Task<IResult> RemoveProduct([FromQuery] int productId, [FromServices] IProductRepository productRepository)
    {
        if (productId <= 0)
        {
            return Results.BadRequest(new { Message = "Invalid product ID" });
        }

        var product = await productRepository.GetAsync(productId);
        if (product == null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        await productRepository.DeleteAsync(productId);

        return Results.Ok(new { Message = "Product removed successfully" });
    }

    public async Task<IResult> UpdateProduct([FromBody] ProductRequest productRequest, [FromQuery] int productId, [FromServices] IProductRepository _product)
    {
        if (productId <= 0)
        {
            return Results.BadRequest(new { Message = "Invalid product ID" });
        }

        if (productRequest == null || string.IsNullOrEmpty(productRequest.Name))
        {
            return Results.BadRequest(new { Message = "Invalid request data" });
        }

        var product = await _product.GetAsync(productId);
        if (product == null)
        {
            return Results.NotFound(new { Message = "Product not found" });
        }

        product.Name = productRequest.Name;
        await _product.UpdateAsync(product);

        return Results.Ok(new { ProductId = product.Id });
    }

}
