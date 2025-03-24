using Microsoft.AspNetCore.Mvc;
using Presentation.Categories.Requests;
using Microsoft.AspNetCore.Http;
using MediatR;
using Application.Categories.Commands;
using Application.Categories.Queries;
using EComputer;

namespace Presentation.Categories;

public class CategoryEndPoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var categoryService = app.MapGroup("api/Category")
            .RequireAuthorization(policy => policy.RequireRole(SD.Role.Admin));

        categoryService.MapGet("/{categoryId}", GetCategory);
        categoryService.MapPost("/", AddCategory);
        categoryService.MapDelete("/{categoryId}", DeleteCategory);
    }
    public async Task<IResult> AddCategory([FromBody] AddCategoryRequest category, ISender sender)
    {
        var response = await sender.Send(new AddCategoryCommand(category.Name));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    public async Task<IResult> DeleteCategory([FromBody] DeleteCategoryCommand category, ISender sender)
    {
        var response = await sender.Send(new DeleteCategoryCommand(category.Id));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }

    public async Task<IResult> GetCategory(int id, ISender sender)
    {
        var response = await sender.Send(new GetByIdCategoryQuery(id));
        return response.IsFailure ? Results.BadRequest(response.Error) : Results.Ok(response);
    }
}