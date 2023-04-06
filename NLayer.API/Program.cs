using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filter;
using NLayer.API.Middleware;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWork;
using NLayer.Repository;
using NLayer.Repository.Redis;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validator;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


var webApplicationFactory = new WebApplicationFactory<Program>();


builder.Services.AddControllers(option => option.Filters.Add(new ValidateFilterAttribute()))
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RedisConnection>(sp =>
{
    RedisConnection redisConnection = new RedisConnection(builder.Configuration.GetConnectionString("Redis"));
    return redisConnection;
});
builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>),typeof(Service<>));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductRepository, CacheProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);

    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomException();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();