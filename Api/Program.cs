using Carter;
using Microsoft.EntityFrameworkCore;
using Infrasctructure.Database;
using Infrasctructure.Middleware;
using Infrasctructure.Cache;
using Infrastrcture.Email;
using Infrasctructure.PasswordHasher;
using Infrasctructure.Jwt;
using Infrasctructure.UnitOfWork;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.Repositories.Classes;
using Api.Middleware;
using Api.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailSettingsService>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ICartItemRepository, CartItemRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICacheEntityService, CacheEntityService>();
builder.Services.AddTransient<IEmailSenderService ,EmailSenderService>();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Configuration.AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRE_CONNECTION_STRING")));

builder.Services.AddStackExchangeRedisCache(x =>
    x.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING"));


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));


builder.Services.AddControllers(options =>
options.Filters.AddService<InputValidationActionFilter>());


builder.Services.AddJwtAuthentication(builder.Configuration);/////////////
builder.Services.AddJwtAuthorization();//////////////

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
