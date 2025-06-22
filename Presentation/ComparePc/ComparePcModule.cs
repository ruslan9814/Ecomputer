//using Application.ComparePc.Queries;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Presentation.ComparePc.Requests;

//namespace Presentation.ComparePc;

//public sealed class ComparePcModule : CarterModule
//{
//    public override void AddRoutes(IEndpointRouteBuilder app)
//    {
//        var compare = app.MapGroup("api/compare-pc");

//        compare.MapPost("/", ComparePcs);
//        compare.MapPost("/by-ids", ComparePcsByIds);
//    }

//    private static async Task<IResult> ComparePcs(
//        [FromBody] ComparePcsRequest request,
//        ISender sender)
//    {
//        if (request == null || request.Pc1 == null || request.Pc2 == null)
//            return Results.BadRequest("Не заданы спецификации ПК.");

//        var response = await sender.Send(new ComparePcsQuery(request.Pc1, request.Pc2));

//        return response.IsFailure
//            ? Results.BadRequest(response.Error)
//            : Results.Ok(response.Value);
//    }

//    private static async Task<IResult> ComparePcsByIds(
//        [FromBody] ComparePcsByIdsRequest request,
//        ISender sender)
//    {
//        if (request == null)
//            return Results.BadRequest("Запрос не задан.");

//        var response = await sender.Send(new ComparePcsByIdsQuery(request.Pc1Id, request.Pc2Id));

//        return response.IsFailure
//            ? Results.BadRequest(response.Error)
//            : Results.Ok(response.Value);
//    }
//}
