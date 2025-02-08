using Carter;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using test.Endpoints.Users.Requests;

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

    private async Task<IResult> GetUser(int id, ISender sender)
    {
        //var response = await sender.Send(new GetUserByIdCommand(id));
        //return response.IsFailure
        //    ? Results.BadRequest(response.Error)
        //    : Results.Ok(response);

        throw new NotImplementedException();
    }

    private async Task<IResult> RegisterUser([FromBody] RegisterUserRequest request, ISender sender)
    {

       var response = await sender.Send(new RegisterUserCommand(request.Email, request.Password, request.Username, request.Address));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> LoginUser([FromBody] LoginUserRequest request, ISender sender)
    {

       var response = await sender.Send(new LoginUserCommand(request.Email, request.Password));
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    private async Task<IResult> UpdateUsers([FromBody] UpdateUserRequest request, ISender sender)
    {

        //var response = await sender.Send(new UpdateProfileCommand(request.));
        //return response.IsFailure
        //    ? Results.BadRequest(response.Error)
        //    : Results.Ok(response);

        throw new NotImplementedException();
    }

    private async Task<IResult> RemoveUsers([FromBody] DeleteUserRequest request, ISender sender)
    {
        var response = await sender.Send(new DeleteUserByIdCommand(request.Id));
        return response.IsFailure ? 
            Results.BadRequest(response.Error) 
            : Results.Ok(response);
    }
}
