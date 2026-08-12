using System.Text;
using GymQuest.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GymQuest.Application.Interfaces;
using GymQuest.Application.Services;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using GymQuest.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<GymQuestDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios (Domain.Interfaces -> Infrastructure.Repositories)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IHeroeRepository, HeroeRepository>();
builder.Services.AddScoped<IEjercicioRepository, EjercicioRepository>();
builder.Services.AddScoped<IRutinaRepository, RutinaRepository>();
builder.Services.AddScoped<ISesionEntrenamientoRepository, SesionEntrenamientoRepository>();
builder.Services.AddScoped<IRachaRepository, RachaRepository>();
builder.Services.AddScoped<ILogroRepository, LogroRepository>();
builder.Services.AddScoped<IMisionRepository, MisionRepository>();

// Servicios de Application (casos de uso)
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRutinaService, RutinaService>();
builder.Services.AddScoped<IEntrenamientoService, EntrenamientoService>();
builder.Services.AddScoped<IHeroeService, HeroeService>();
builder.Services.AddScoped<IRachaService, RachaService>();
builder.Services.AddScoped<ILogroService, LogroService>();
builder.Services.AddScoped<IMisionService, MisionService>();

// Hasher de contraseñas (solo el componente de Identity, sin el sistema completo)
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();