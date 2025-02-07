using Carter;
using Microsoft.AspNetCore.Mvc;
using Test.Endpoints.Users.Requests;

namespace test.Endpoints.Users;

public sealed class UserEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("api/users");

        user.MapGet("/{UserId}", GetUser);
        user.MapPost("/register", RegisterUser);
        user.MapPost("/login", LoginUser);
        user.MapPut("/{UserId}", UpdateUsers);
        user.MapDelete("/{UserId}", RemoveUsers);
    }

    private async Task<IResult> GetUser([FromBody] GetUserRequest getUserRequest, [FromServices] ISender sender)
    {
        var response = await sender.Send(new GetUserById(getUserRequest.Id));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> RegisterUser([FromBody] RegistUserRequest userRequest, [FromServices] ISender sender)
    {

       var response = await sender.Send(new AddUser(userRequest));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> LoginUser([FromBody] LoginUserRequest loginRequest, [FromServices] ISender sender)
    {

       var response = await sender.Send(new LoginUser(loginRequest));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> UpdateUsers([FromBody] UpdateUserRequest updateUserRequest, [FromServices] ISender sender)
    {

        var response = await sender.Send(new UpdateUser(updateUserRequest));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> RemoveUsers([FromBody] RemoveUserRequest removeUserRequest, [FromServices] ISender sender)
    {
        var response = await sender.Send(new RemoveUser(removeUserRequest.Id));
        return response.IsFailure ? 
            Results.BadRequest(response.Error) 
            : Results.Ok(response);
    }
}
