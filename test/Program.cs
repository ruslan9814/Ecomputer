using Carter;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Test.Database.Repositories.Classes;
using Test.Database.Repositories.Interfaces;
using Test.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Test.Infrastrcture.Jwt;
using Test.Middleware;
using Test;
using Test.Models;
using Test.Extensions;

var builder = WebApplication.CreateBuilder(args);

//TODO
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly());

builder.Configuration.AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRE_CONNECTION_STRING")));

builder.Services.AddStackExchangeRedisCache(x =>
    x.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING"));

builder.Services.AddOptions<JwtOptions>("Jwt");

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<InputValidationActionFilter>();////////////////////////////podumat
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });


builder.Services.AddAuthorization(x =>
{
    x.AddPolicy(SD.Role.User, policy =>
    {
        policy.RequireRole(Role.User.GetName());
    });

    x.AddPolicy(SD.Role.Admin, policy =>
    {
        policy.RequireRole(Role.Admin.GetName());
    });

    x.AddPolicy(SD.Role.UserAndAdmin, policy =>
    {
        policy.RequireRole(
        Role.User.GetName(), Role.Admin.GetName());
    });
});

builder.Services.AddCarter();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<GlobalHandlingExpcetionMiddleware>();////////////////////////////////////////////

app.MigrateDbContext<ApplicationDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();


app.MapCarter();

app.Run();
