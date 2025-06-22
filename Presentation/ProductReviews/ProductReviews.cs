using Application.ProductReviews.Commands;
using Application.ProductReviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.ProductReviews.Requests;

namespace Presentation.ProductReviews;

public sealed class ProductReviews : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var productReviews = app.MapGroup("api/productreviews")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin, SD.Role.User));

        //productReviews.MapGet("/{id}", GetProductReviewById);
        productReviews.MapGet("/{productId}", GetProductReviewsByProductId);
        productReviews.MapPost("/", AddProductReview);
        productReviews.MapPut("/", UpdateProductReview);
        productReviews.MapDelete("/{id}", DeleteProductReview);
        productReviews.MapGet("/top-rated", GetTopRatedProducts);
    }


    //private static async Task<IResult> GetProductReviewById(int id, HttpContext context, 
    //    ISender sender)
    //{
    //    if (!TryGetUserId(context, out int userId))
    //    {
    //        return Results.Unauthorized();
    //    }
    //    var response = await sender.Send(new GetReviewByIdQuery(id, userId));
    //    return response.IsFailure
    //        ? Results.BadRequest(response.Error)
    //        : Results.Ok(response.Value);
    //}

    private static async Task<IResult> GetProductReviewsByProductId(int productId, 
        HttpContext context, ISender sender)
    {
        var response = await sender.Send(new GetReviewsByProductIdQuery(productId));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response.Value);
    }

    private async Task<IResult> AddProductReview(CreateProductReviewRequest request, ISender sender)
    {

        var response = await sender.Send(new CreateProductReviewCommand(
            request.ProductId,
            request.Rating,
            request.ReviewText));

        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> UpdateProductReview(HttpContext context, 
        UpdateProductReviewRequest request, ISender sender)
    {

        var response = await sender.Send(new UpdateProductReviewCommand(
            request.Id,
            request.Content,
            request.Rating));

        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> DeleteProductReview(int id, ISender sender)
    {

        var response = await sender.Send(new DeleteProductReviewCommand(id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> GetTopRatedProducts(ISender sender)
    {
        var response = await sender.Send(new GetTopProductsByRatingQuery());
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response.Value);
    }
}
