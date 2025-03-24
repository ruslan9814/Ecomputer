using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public class InputValidationActionFilter(ILogger<InputValidationActionFilter> logger)
{
    private readonly ILogger<InputValidationActionFilter> _logger = logger;

    public async void OnActionExecuting(ActionExecutingContext context) // это нужно исправить и сделать чтобы правильно работало
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var errorDetails = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );


        _logger.LogWarning("Invalid model state for action {Action}. Errors: {Errors}",
             context.ActionDescriptor.DisplayName,
             errorDetails);


        var response = new
        {
            success = false,
            Message = "Invalid model state",
            Errors = errorDetails
        };


        context.Result = new JsonResult(response)
        {
            StatusCode = 400
        };

        await Task.CompletedTask;

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {


    }

}