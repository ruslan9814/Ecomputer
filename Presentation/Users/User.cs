using Application.Users.Commands;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Users.Requests;
 

namespace Presentation.Users;

public sealed class User : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("api/user");

        user.MapGet("/{Id}", GetUser);
        user.MapPost("/register", RegisterUser);
        user.MapPost("/login", LoginUser);
        user.MapPut("/", UpdateUsers);  
        user.MapDelete("/", RemoveUsers);  
        user.MapGet("/confirm-email/{token}", ConfirmEmail);
        user.MapPost("/logout", Logout);
        user.MapPost("/refresh-token", UpdateRefreshToken);  
        user.MapGet("/", GetAllUsers);
        user.MapPost("/upload-image", UploadImage)
        .DisableAntiforgery()
        .RequireAuthorization();
    }

    private static async Task<IResult> ConfirmEmail([FromRoute] string token,
        [FromQuery] string returnUrl, ISender sender)
    {
        var response = await sender.Send(new ConfirmEmailCommand(token));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Redirect(returnUrl);
    }

    public async Task<IResult> GetUser([FromRoute] int id, ISender sender)
    {
        var response = await sender.Send(new GetUserQuery(id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response.Value);
    }
    private static async Task<IResult> RegisterUser([FromBody] RegisterUserRequest request,
        ISender sender)
    {
        var response = await sender.Send(new RegisterUserCommand(
            request.Email, request.Password, request.Name,
            request.Address, request.Role, request.returnUrl));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok();
    }

 
    private static async Task<IResult> LoginUser([FromBody] LoginUserRequest request,
        ISender sender)
    {
        var response = await sender.Send(new LoginUserCommand(request.Email, request.Password));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response.Value);
    }

 
    private static async Task<IResult> UpdateUsers([FromBody] UpdateUserRequest request,
        ISender sender)
    {
        var response = await sender.Send(new UpdateProfileCommand(request.Id, request.Name,
            request.Address, request.Password));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }
     
    private static async Task<IResult> RemoveUsers([FromBody] DeleteUserRequest request,
        ISender sender)
    {
        var response = await sender.Send(new DeleteUserByIdCommand(request.Id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

 
    private static async Task<IResult> Logout([FromBody] LogoutUserRequest logout, ISender sender)
    {
        var response = await sender.Send(new LogoutUserCommand(logout.Token));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

 
    private static async Task<IResult> UpdateRefreshToken([FromBody]
        UpdateRefreshTokenRequest request, ISender sender)
    {
        var response = await sender.Send(new UpdateRefreshTokenCommand(request.Token,
            request.RefreshToken));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> GetAllUsers(ISender sender)
    {
        var response = await sender.Send(new GetAllUserQuery());
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private static async Task<IResult> UploadImage([FromForm] ImageUploadRequest request, ISender sender)
    {
        if (request.ImageFile == null || request.ImageFile.Length == 0)
        {
            return Results.BadRequest("Файл изображения не предоставлен.");
        }

        var response = await sender.Send(new UploadUserImageCommand(request.ImageFile));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response.Value);
    }
}