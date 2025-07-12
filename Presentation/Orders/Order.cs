using Application.OrderItems.Command;
using Application.Orders.Queries;
using Domain.Orders;
using Infrasctructure.CurrentUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Presentation.Orders.Requests;
using System.Security.Claims;

namespace Presentation.Orders;

public sealed class Order : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var order = app.MapGroup("api/order")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin, SD.Role.User));
        order.MapPut("/order-status/{id}", UpdateOrderStatus);
        order.MapGet("/get-orderHistory/", GetHistoryOrder);
        order.MapGet("/get-top-products/", GetTopProduct);
        order.MapGet("/get-order-statistics/", GetOrderStatistics);
    }
   
    private static async Task<IResult> UpdateOrderStatus(
     int id, UpdateOrderStatusRequest request, ISender sender)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), request.Status))
        {
            return Results.BadRequest("Недопустимый статус.");
        }

        var status = (OrderStatus)request.Status;

        var result = await sender.Send(new UpdateOrderStatusCommand(id, status));
        return result.IsFailure
            ? Results.BadRequest(result.Error)
            : Results.Ok("Статус заказа успешно обновлён.");
    }

    private static async Task<IResult> GetHistoryOrder(
     ISender sender,
     ICurrentUserService currentUserService,
     HttpContext httpContext)   
    {
        int userId = currentUserService.UserId;
        var user = httpContext.User;

        bool isAdmin = user.IsInRole(SD.Role.Admin);

        int? queryUserId = isAdmin ? null : userId;

        var response = await sender.Send(new GetOrderHistoryQuery(queryUserId));

        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }



    private static async Task<IResult> GetTopProduct(ISender sender)
    {
        var response = await sender.Send(new GetTopProductsQuery());
        return response.IsFailure
            ? Results.BadRequest(response.Error)
            : Results.Ok(response);
    }

    public static async Task<IResult> GetOrderStatistics(ISender sender)
    {
        var response = await sender.Send(new GetOrderStatisticsQuery());
        return response.IsFailure
           ? Results.BadRequest(response.Error)
           : Results.Ok(response);
    }

}
