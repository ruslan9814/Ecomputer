using Carter;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using test.Database;
using test.Database.Repositories.Classes;
using test.Database.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddYamlFile(
    "appsettings.yml", optional: true, reloadOnChange: true);


builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRE_CONNECTION_STRING")));

builder.Services.AddStackExchangeRedisCache(x =>
    x.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCarter();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MigrateDbContext<ApplicationDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.MapCarter();


app.Run();

