using Infrasctructure.BlackList;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Middleware;

public class TokenBlackListMiddleware(RequestDelegate next, IBlackListService blackList)
{
    private readonly RequestDelegate _next = next;
    private readonly IBlackListService _blackList = blackList;

    public async Task Invoke(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            await _next(context);
            return;
        }

        var token = authHeader["Bearer ".Length..].Trim();

        if (!IsValidJwt(token))
        {

            await _next(context);
            return;
        }

        if (await _blackList.IsExistsToken(token))
        {

            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Token has been revoked");
            return;
        }

        await _next(context);
    }

    private static bool IsValidJwt(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.CanReadToken(token);

        return jwtToken;
    }
}
