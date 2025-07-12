using Application.Favorite.Command;
using Application.Favorite.Quieries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Favorites.Requests;



namespace Presentation.Favorites;

public sealed class Favorite : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var favorite = app.MapGroup("api/favorite")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin, SD.Role.User));
        favorite.MapGet("/", GetAllFavorites);
        favorite.MapPost("/", AddFavorite);
        favorite.MapDelete("/{productId}", DeleteFavorite);

    }

    private static async Task<IResult> GetAllFavorites(HttpContext context,  ISender sender)
    {

        var response = await sender.Send(new GetAllFavoriteQuery());
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> AddFavorite(HttpContext httpContext,
        [FromBody] AddFavoriteRequest request, ISender sender)
    {

        var response = await sender.Send(new AddFavoritesCommand(request.ProductId));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }
    private static async Task<IResult> DeleteFavorite(HttpContext httpContext, int productId, ISender sender)
    {
        var response = await sender.Send(new DeleteFavoritesCommand(productId));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }


}
