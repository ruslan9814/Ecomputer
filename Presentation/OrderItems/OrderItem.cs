using Application.OrderItems.Command;
using Application.OrderItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.OrderItems.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace Presentation.OrderItems;

public sealed class OrderItem : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var orderItem = app.MapGroup("api/orderitem")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin, SD.Role.User));

        orderItem.MapGet("/", GetOrderItem);
        orderItem.MapPost("/", CreateOrderItem);
    }


    private static async Task<IResult> GetOrderItem(ISender sender)
    {

        var res = await sender.Send(new GetOrderItemsQuery());
        return res.IsFailure ? Results.BadRequest(res.Error) : Results.Ok(res.Value);
    }

    private static async Task<IResult> CreateOrderItem(AddOrderItemRequest request, ISender sender)
    {
        var response = await sender.Send(new AddOrderItemCommand(request.CartId));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok();
    }

}
