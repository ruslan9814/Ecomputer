using Carter;
using Microsoft.AspNetCore.Mvc;
using Test.Services.UserService;
using Test.Endpoints.Users.Requests;

namespace Test.Endpoints.Users;

public sealed class UserEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("api/users");

        user.MapGet("/{UserId}", GetUsers);
        user.MapPost("/register", RegisterUser);
        user.MapPost("/login", LoginUser);
        user.MapPut("/{UserId}", UpdateUsers);
        user.MapDelete("/{UserId}", RemoveUsers);
    }

    [HttpPost("getUser")]
    private async Task<IResult> GetUsers([FromBody] GetUserRequest getUserRequest, [FromServices] IUserService userService)
    {
        var result = await userService.GetUser(getUserRequest);
        if (result is null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        return Results.Ok(result);
    }

    [HttpPut("reqister")]
    private async Task<IResult> RegisterUser([FromBody] RegistUserRequest userRequest, [FromServices] IUserService userService)
    {   

        if (userRequest is { Username: not null})
        {
            return Results.BadRequest(new { Message = "Invalid user request data." });
        }

        var userId = await userService.AddUser(userRequest);
        return Results.Created($"/api/users/{userId}", new { UserId = userId });
    }

    [HttpPost("login")]
    private async Task<IResult> LoginUser([FromBody] LoginUserRequest loginRequest, [FromServices] IUserService userService)
    {
 
        var user = await userService.LoginUser(loginRequest);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new { Message = "Login successful", UserId = user.Id });
    }

    [HttpPut("update")]
    private async Task<IResult> UpdateUsers([FromBody] UpdateUserRequest updateUserRequest, [FromServices] IUserService userService)
    {

        var updated = await userService.UpdateUser(updateUserRequest);
        if (!updated)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        return Results.Ok(new { Message = "User updated successfully" });
    }

    [HttpDelete("remove")]
    private async Task<IResult> RemoveUsers([FromBody] RemoveUserRequest removeUserRequest, [FromServices] IUserService userService)
    {
        var removed = await userService.DeleteUser(removeUserRequest);
        if (!removed)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        return Results.Ok(new { Message = "User removed successfully" });
    }
}
