using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Test.Endpoints.Users.Requests;

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

        if (context.ActionDescriptor.RouteValues["action"] != "LoginUser")
        {
            return;
        }

        var body = context.ActionArguments.Values.FirstOrDefault();
        if (body is LoginUserRequest loginRequest)
        {

            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                var responseObj = new
                {
                    successful = false,
                    error = "The input is not valid",
                    details = "Email and password must not be empty"
                };

                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = 400
                };
            }
        }


        if (!context.ModelState.IsValid)
        {
            var responseObj = new
            {
                successful = false,
                error = "The input is not valid",
                details = context.ModelState.Values.ToList()
            };

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = 400
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {


    }
}