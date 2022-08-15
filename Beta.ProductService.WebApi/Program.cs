using Beta.ProductService.WebApi.Common;
using Beta.ProductService.WebApi.EntityServices;
using Beta.ProductService.WebApi.Interfaces;
using Beta.ProductService.WebApi.Persistance.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

builder.Services.AddDbContext<SqlDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddScoped<ISqlDbContext, SqlDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleWare>();

app.UseAuthorization();

app.MapControllers();

app.Run();