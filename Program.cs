using Aplicacion;
using Infraestructura;
using Microsoft.EntityFrameworkCore;
using Nucleo.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuración de DbContext para Railway (MySQL)
builder.Services.AddDbContext<TiendaDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Railway")
        ?? throw new InvalidOperationException("La cadena de conexión 'Railway' no está configurada.");
    options.UseMySQL(connectionString);
});

// Inyección de dependencias
builder.Services.AddScoped<IRepositorioProducto, RepositorioProducto>();
builder.Services.AddScoped<IRepositorioOrden, RepositorioOrden>();
builder.Services.AddScoped<IRepositorioOrderItem, RepositorioOrderItem>();
builder.Services.AddScoped<ServiciosProducto>();
builder.Services.AddScoped<ServiciosOrden>();

// CORS: Solo permite la IP 187.155.101.200
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificIp", policy =>
    {
        policy.WithOrigins("http://187.155.101.200", "https://187.155.101.200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(
                jwtSettings["Key"] ?? throw new InvalidOperationException("La clave JWT 'Key' no está configurada en appsettings.json.")
            )
        )
    };

    // Permitir que el token se envíe como ApiKey (sin 'Bearer')
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(token) && !token.Trim().StartsWith("Bearer "))
            {
                context.Token = token.Trim();
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});


// Swagger con soporte para JWT en Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tienda API", Version = "v1" });
    options.AddSecurityDefinition("JWT", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese solo el token JWT (sin 'Bearer')"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "JWT"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddOpenApi();

var app = builder.Build();

// Swagger y CORS
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Aplica la política CORS solo para la IP específica
app.UseCors("AllowSpecificIp");

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
