using System.Text;
using GymQuest.Api.Configuration;
using GymQuest.Api.Filters;
using GymQuest.Api.Middleware;
using GymQuest.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GymQuest.Application.Interfaces;
using GymQuest.Application.Services;
using GymQuest.Application.Validators;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using GymQuest.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

// Carga el archivo .env (si existe) e inyecta sus pares clave=valor como
// variables de entorno del proceso ANTES de que CreateBuilder lea la config.
// En producción normalmente no habrá .env y esta línea simplemente no hace nada.
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ===================== Controllers + filtros =====================
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

// ===================== OpenAPI / Scalar =====================
// El transformer agrega la definición del esquema Bearer al documento,
// para que Scalar dibuje el botón de "Authorize" (por defecto no aparece).
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// ===================== Persistencia =====================
builder.Services.AddDbContext<GymQuestDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===================== Repositorios (Domain.Interfaces -> Infrastructure.Repositories) =====================
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IHeroeRepository, HeroeRepository>();
builder.Services.AddScoped<IEjercicioRepository, EjercicioRepository>();
builder.Services.AddScoped<IRutinaRepository, RutinaRepository>();
builder.Services.AddScoped<ISesionEntrenamientoRepository, SesionEntrenamientoRepository>();
builder.Services.AddScoped<IRachaRepository, RachaRepository>();
builder.Services.AddScoped<ILogroRepository, LogroRepository>();
builder.Services.AddScoped<IMisionRepository, MisionRepository>();

// ===================== Servicios de Application (casos de uso) =====================
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRutinaService, RutinaService>();
builder.Services.AddScoped<IEntrenamientoService, EntrenamientoService>();
builder.Services.AddScoped<IHeroeService, HeroeService>();
builder.Services.AddScoped<IRachaService, RachaService>();
builder.Services.AddScoped<ILogroService, LogroService>();
builder.Services.AddScoped<IMisionService, MisionService>();

// ===================== Seguridad: hasher + JWT =====================
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// ===================== FluentValidation =====================
// Registra automáticamente TODOS los validators que existan en el ensamblado
// de Application (CrearUsuarioDtoValidator, LoginDtoValidator, etc.) — no hay
// que registrarlos uno por uno a mano.
builder.Services.AddValidatorsFromAssemblyContaining<CrearUsuarioDtoValidator>();

// ===================== Manejo global de errores =====================
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ===================== CORS (para que la app MAUI pueda consumir la API) =====================
const string PoliticaCorsApp = "PoliticaCorsApp";
builder.Services.AddCors(options =>
{
    options.AddPolicy(PoliticaCorsApp, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ===================== A partir de aquí, el contenedor de DI queda sellado =====================
var app = builder.Build();

// ===================== Pipeline HTTP (middlewares) =====================
// El manejador de excepciones va PRIMERO en el pipeline: así envuelve
// también los errores que puedan ocurrir en el resto de middlewares.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // UI disponible en /scalar/v1
}

app.UseHttpsRedirection();

app.UseCors(PoliticaCorsApp);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();