using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Middleware;

public class InputValidationActionFilter(ILogger<InputValidationActionFilter> logger) 
    : IAsyncActionFilter
{
    private readonly ILogger<InputValidationActionFilter> _logger = logger;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, 
        ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errorDetails = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            _logger.LogWarning("Invalid model state for action {Action}. Errors: {@Errors}",
                context.ActionDescriptor.DisplayName,
                errorDetails);

            var response = new
            {
                success = false,
                message = "Invalid model state",
                errors = errorDetails
            };

            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            return; 
        }

        await next();

    }


    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
