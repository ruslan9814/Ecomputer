////когда удаляем миграцию InitialCreate то нужно удалять и ApplicationDbContextModelSnapshot

using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Database;
using test.Database.Repositories.Interfaces;
using test.Endpoints.Carts.Models;
using test.Endpoints.Users.Models;
using test.Models;

namespace test.EndPoints;

public sealed class UserEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("api/users");
        user.MapGet("{UserId}/", GetUsers);           
        user.MapPost("/", AddUsers);                
        user.MapPut("{UserId}/", UpdateUsers);     
        user.MapDelete("{UserId}/", RemoveUsers); 
    }


    public async Task<IResult> GetUsers([FromRoute] int UserId, [FromServices] IUserRepository _user)
    {
        if (UserId <= 0)
        {
            return Results.BadRequest(new { Message = "Invalid user ID" });
        }

        var user = await _user.GetAsync(UserId);
        if (user == null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        var response = new UserResponse
        {
            Id = user.Id,
            Username = user.Name
        };

        return Results.Ok(response);
    }


    public async Task<IResult> AddUsers([FromBody] UserRequest userRequest, [FromServices] IUserRepository _user)
    {
        if (userRequest == null || string.IsNullOrEmpty(userRequest.Username))
        {
            return Results.BadRequest(new { Message = "Invalid request data" });
        }

        var cart = new Cart();
        var user = new User
        {
            Name = userRequest.Username,
            Cart = cart
        };

        await _user.AddAsync(user);

        return Results.Ok(new { UserId = user.Id, user.Name });
    }

    public async Task<IResult> UpdateUsers([FromBody] UserRequest userRequest, [FromQuery] int UserId, [FromServices] IUserRepository _user)
    {
        if (UserId <= 0)
        {
            return Results.BadRequest(new { Message = "Invalid user ID" });
        }

        if (userRequest == null || string.IsNullOrEmpty(userRequest.Username))
        {
            return Results.BadRequest(new { Message = "Invalid request data" });
        }

        var user = await _user.GetAsync(UserId);
        if (user == null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        user.Name = userRequest.Username;
        await _user.UpdateAsync(user);

        return Results.Ok(new { UserId = user.Id });
    }

    public async Task<IResult> RemoveUsers([FromQuery] int UserId, [FromServices] IUserRepository _user)
    {
        if (UserId <= 0)
        {
            return Results.BadRequest(new { Message = "Invalid user ID" });
        }

        var user = await _user.GetAsync(UserId);
        if (user == null)
        {
            return Results.NotFound(new { Message = "User not found" });
        }

        await _user.DeleteAsync(UserId);

        return Results.Ok(new { Message = "User removed successfully" });
    }


}

