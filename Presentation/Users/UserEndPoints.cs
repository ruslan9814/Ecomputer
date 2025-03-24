using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Application.Users.Commands;
using MediatR;
using Application.Users.Queries;
using Presentation.Users.Requests;

namespace Presentation.Users;

public sealed class UserEndPoints : CarterModule
{
    [HttpGet]
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("api/user");

        user.MapGet("/{UserId}", GetUser);
        user.MapPost("/register", RegisterUser);
        user.MapPost("/login", LoginUser);
        user.MapPut("/{UserId}", UpdateUsers);
        user.MapDelete("/{UserId}", RemoveUsers);
        user.MapGet("/confirm-email/{token}", ConfirmEmail);
        user.MapPost("/logout", Logout);
    }

    public async Task<IResult> ConfirmEmail(string token, string returnUrl, ISender sender)
    {
        var response = await sender.Send(new ConfirmEmailCommand(token));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Redirect(returnUrl);
    }

    public async Task<IResult> GetUser(int id, ISender sender)
    {
        var response = await sender.Send(new GetUserQuery(id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response.Value);
    }

    public async Task<IResult> RegisterUser([FromBody] RegisterUserRequest request, ISender sender)
    {
        var response = await sender.Send(new RegisterUserCommand(request.Email, request.Password,
            request.Username, request.Address, request.Role, request.returnUrl));

        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok();
    }

    public async Task<IResult> LoginUser([FromBody] LoginUserRequest request, ISender sender)
    {

        var response = await sender.Send(new LoginUserCommand(request.Email, request.Password));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response.Value);
    }

    public async Task<IResult> UpdateUsers([FromBody] UpdateUserRequest request, ISender sender)
    {

        var response = await sender.Send(new UpdateProfileCommand(request.Id, request.Name,
            request.Address));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    public async Task<IResult> RemoveUsers([FromBody] DeleteUserRequest request, ISender sender)
    {
        var response = await sender.Send(new DeleteUserByIdCommand(request.Id));
        return response.IsFailure ?
            Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    public async Task<IResult> Logout(LogoutUserRequest logout,ISender sender)///////////////////
    {
        var response = await sender.Send(new LogoutUserCommand(logout.Token));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }
}
