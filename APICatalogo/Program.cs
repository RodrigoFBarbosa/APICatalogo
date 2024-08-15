using APICatalogo.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Repository;
using APICatalogo.DTOs.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter)); // adicionando o filtro como global
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; //vai ignorar quando houver uma referencia ciclica
}).AddNewtonsoftJson();
        
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//adscoped cria uma instancia unica por request 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var lendoConfig = builder.Configuration["key1"];
var lendoConfig2 = builder.Configuration["section1:key1"];

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ApiLoggingFIlter>();

builder.Services.AddAutoMapper(typeof(ProductDTOMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler(); // meu metodo em ApiExceptionMiddlewareExtensions
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
