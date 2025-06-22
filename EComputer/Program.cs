using Carter;
using Microsoft.EntityFrameworkCore;
using Infrasctructure.Database;
using Infrasctructure.Cache;
using Infrastrcture.Email;
using Infrasctructure.PasswordHasher;
using Infrasctructure.Jwt;
using Infrasctructure.UnitOfWork;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.Repositories.Classes;
using Api.Extensions;
using Presentation.Users;
using Api.Middleware;
using Infrasctructure.BlackList;
using Application.Orders.Queries;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Infrasctructure.Email;
using Infrasctructure.CurrentUser;
using Infrasctructure.BlobStorage;
 


var builder = WebApplication.CreateBuilder(new WebApplicationOptions());
 
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000")  
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
    });
});

builder.Services.Configure<EmailSettingsService>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ICartItemRepository, CartItemRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddTransient<IFavoritesRepository, FavoritesRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICacheEntityService, CacheEntityService>();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddTransient<IBlackListService, BlackListService>();
builder.Services.AddTransient<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<IBlobService, BlobService>();




builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(GetOrderCommand).Assembly));

builder.Configuration.AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")));


builder.Services.AddStackExchangeRedisCache(x =>
    x.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING"));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers(options =>
    options.Filters.AddService<InputValidationActionFilter>());


builder.Services.AddJwtAuthentication(builder.Configuration); 
builder.Services.AddJwtAuthorization();

builder.Services.AddCarter(
    new DependencyContextAssemblyCatalog(
        [
            typeof(User).Assembly
        ]));

builder.Services
       .AddApiVersioning(options => options.ApiVersionReader = new UrlSegmentApiVersionReader()) 
       .AddApiExplorer(options =>                                                               
       {
           options.GroupNameFormat = "'v'VVV";
           options.SubstituteApiVersionInUrl = true;
       });

var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};

var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
};

var contact = new OpenApiContact()
{
    Name = "Mohamad Lawand",
    Email = "hello@mohamadlawand.com",
    Url = new Uri("http://www.mohamadlawand.com")
};

var license = new OpenApiLicense()
{
    Name = "Free License",
    Url = new Uri("http://www.mohamadlawand.com")
};

var info = new OpenApiInfo()
{
    Version = "v1",
    Title = "Minimal API - JWT Authentication with Swagger demo",
    Description = "Implementing JWT Authentication in Minimal API",
    TermsOfService = new Uri("http://www.example.com"),
    Contact = contact,
    License = license
};

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", info);
    o.AddSecurityDefinition("Bearer", securityScheme);
    o.AddSecurityRequirement(securityReq);
});

var app = builder.Build();


app.UseMiddleware<GlobalHandlingExpcetionMiddleware>(); 


app.UseCors("AllowFrontend");


app.UseMiddleware<TokenBlackListMiddleware>(); 


app.MigrateDbContext<ApplicationDbContext>();  


//if (app.Environment.IsDevelopment())
//{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthentication();
app.UseAuthorization();
 
app.MapCarter();

app.Run();

