using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Api.Extensions;
public static class AuthenticationExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, 
        IConfiguration configuration)
    {

     
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RoleClaimType = ClaimTypes.Role/// this is the line that was added
                };
            });
    }

    public static void AddJwtAuthorization(this IServiceCollection services) => 
        services.AddAuthorizationBuilder()
            .AddPolicy(SD.Role.Admin, policy => policy.RequireRole(SD.Role.Admin))
            .AddPolicy(SD.Role.User, policy => policy.RequireRole(SD.Role.User))
            .AddPolicy(SD.Role.UserAndAdmin, policy => policy.RequireRole(SD.Role.User, 
                SD.Role.Admin));
}




//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//    };
//});


//builder.Services.AddAuthorization(x =>
//{
//    x.AddPolicy(SD.Role.User, policy =>
//    {
//        policy.RequireRole(Role.User.GetName());
//    });

//x.AddPolicy(SD.Role.Admin, policy =>
//{
//    policy.RequireRole(Role.Admin.GetName());
//});

//x.AddPolicy(SD.Role.UserAndAdmin, policy =>
//{
//    policy.RequireRole(
//    Role.User.GetName(), Role.Admin.GetName());
//});
//});