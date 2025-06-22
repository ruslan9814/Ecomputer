using Microsoft.AspNetCore.Mvc;
using Presentation.Categories.Requests;
using Microsoft.AspNetCore.Http;
using MediatR;
using Application.Categories.Commands;
using Application.Categories.Queries;

namespace Presentation.Categories;

public class Category : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var categoryService = app.MapGroup("api/category")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin, SD.Role.User));

        categoryService.MapGet("/{id}", GetCategory);
        categoryService.MapGet("/", GetAllCategory);
        categoryService.MapPost("/", AddCategory);
        categoryService.MapDelete("/", DeleteCategory);
    }
    private static async Task<IResult> AddCategory([FromBody] AddCategoryRequest category,
        ISender sender)
    {
        var response = await sender.Send(new AddCategoryCommand(category.Name));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> DeleteCategory([FromBody] DeleteCategoryRequest category,
        ISender sender)
    {
        var response = await sender.Send(new DeleteCategoryCommand(category.Id));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> GetCategory(int id, ISender sender)
    {
        var response = await sender.Send(new GetByIdCategoryQuery(id));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    private static async Task<IResult> GetAllCategory(ISender sender)
    {
        var response = await sender.Send(new GetAllCategoriesQuery());
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }
}