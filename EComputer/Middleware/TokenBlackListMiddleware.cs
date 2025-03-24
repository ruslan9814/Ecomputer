using Infrasctructure.BlackList;

namespace Api.Middleware;

public class TokenBlackListMiddleware(RequestDelegate next, IBlackListService blackList)
{
    private readonly RequestDelegate _next = next;
    private readonly IBlackListService _blackList = blackList;

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();//реализовать по другому
        if (token is null)
        {
            await _next(context);
            return;
        }

        bool isBlackList = await _blackList.IsExistsToken(token);

        if (isBlackList)
        {
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
    }
}
