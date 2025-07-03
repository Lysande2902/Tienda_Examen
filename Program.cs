using Aplicacion;
using Infraestructura;
using Microsoft.EntityFrameworkCore;
using Nucleo.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuración de DbContext para Railway (MySQL)
builder.Services.AddDbContext<TiendaDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Railway")));

// Inyección de dependencias
builder.Services.AddScoped<IRepositorioProducto, RepositorioProducto>();
builder.Services.AddScoped<IRepositorioOrden, RepositorioOrden>();
builder.Services.AddScoped<IRepositorioOrderItem, RepositorioOrderItem>();
builder.Services.AddScoped<ServiciosProducto>();
builder.Services.AddScoped<ServiciosOrden>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Swagger y CORS
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();
    if (path != null && path.StartsWith("/api/auth/login"))
    {
        await next();
        return;
    }
    if (!context.Request.Headers.TryGetValue("X-Token", out var token) || token != "TOKEN_SUPER_SECRETO")
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Token inválido o ausente");
        return;
    }
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
