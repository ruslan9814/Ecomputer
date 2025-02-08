using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Test.Middleware;

public class InputValidationActionFilter(ILogger<InputValidationActionFilter> logger) : IActionFilter
{
    private readonly ILogger<InputValidationActionFilter> _logger = logger;

    //public void OnActionExecuting(ActionExecutingContext context)
    //{
    //    if (!context.ModelState.IsValid)
    //    {

    //        var responseObj = new
    //        {
    //            successful = false,
    //            error = "The input is not valid",
    //            details = context.ModelState.Values.ToList()
    //        };

    //        context.Result = new JsonResult(responseObj)
    //        {
    //            StatusCode = 400
    //        };
    //    }
    //}

    public void OnActionExecuting(ActionExecutingContext context) // это нужно исправить и сделать чтобы правильно работало
    {

      
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {


    }
}