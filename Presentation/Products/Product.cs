using Microsoft.AspNetCore.Mvc;
using Application.Endpoints.Products.Requests;
using Microsoft.AspNetCore.Http;
using MediatR;
using Application.Products.Queries;
using Application.Products.Commands;
using Presentation.Products.Requests;

namespace Presentation.Products;

////realizovat repositoriy cart and cartItem METODI DLA KORZINI ADD DELETE UPDATE NAPISAT V CLASSE cartItemRepository,
//// TAKJE DOVAVIT cartItemEndPoints i v ney vizvat metodi cerez ICartItemRepository

public sealed class Product : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var product = app.MapGroup("api/product")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin, SD.Role.User));
        product.MapGet("{id}/", GetProduct);
        product.MapGet("/", GetFilterProducts);
        product.MapPost("/", AddProduct).DisableAntiforgery();
        product.MapDelete("/{id}", RemoveProduct); 
        product.MapPut("/{id}", UpdateProduct).DisableAntiforgery();
        product.MapGet("all/", GetAllProducts);

        app.MapGet("api/image-proxy", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
        {
            var url = context.Request.Query["url"].ToString();

            if (string.IsNullOrWhiteSpace(url))
                return Results.BadRequest("Missing image URL");

            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return Results.StatusCode((int)response.StatusCode);

                var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/jpeg";
                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                return Results.File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ImageProxy] Error: {ex.Message}");
                return Results.Problem("Internal server error");
            }
        });
    }

    private static async Task<IResult> GetFilterProducts(string name, decimal minPrice, 
        decimal maxPrice, bool isInStock, int categoryId, ISender sender)
    {
        var response = await sender.Send(new GetFilterProductQuery(name, minPrice, maxPrice, 
            isInStock, categoryId));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> GetProduct(int id, ISender sender)
    {
        var response = await sender.Send(new GetProductByIdQuery(id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> AddProduct([FromForm] AddProductRequest request,
        ISender sender)
    {
        var response = await sender.Send(new AddProductCommand(
            request.Name, 
            request.Price, 
            request.Quantity, 
            request.IsInStock, 
            DateTime.UtcNow, 
            request.CategoryId, 
            request.Description,
            request.ImageFile));

        return response.IsFailure ?
            Results.BadRequest(response.Error) 
            : Results.Ok(response);
    }

    private static async Task<IResult> RemoveProduct([FromBody] DeleteProductRequest request, 
        [FromServices] ISender sender)
    {
         var response = await sender.Send(new DeleteProductCommand(request.Id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> UpdateProduct([FromForm] UpdateProductRequest request, 
        [FromServices] ISender sender)
    {
       var response = await sender.Send(new UpdateProductCommand(request.Id, request.Name,
            request.Description, request.Price, request.Quantity, request.IsInStock,
            request.CategoryId, request.ImageFile));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> GetAllProducts(ISender sender)
    {
        var response = await sender.Send(new GetAllProductsQuery());
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }
}
