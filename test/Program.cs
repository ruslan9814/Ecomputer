using Carter;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Test.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Test.Infrastrcture.Jwt;
using Test.Middleware;
using Test;
using Test.Models;
using Test.Extensions;
using test.Infrastrcture.Email;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<EmailSettingsService>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSenderService ,EmailSenderService>();

//TODO
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Configuration.AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRE_CONNECTION_STRING")));

builder.Services.AddStackExchangeRedisCache(x =>
    x.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING"));

builder.Services.AddOptions<JwtOptions>("Jwt");

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
