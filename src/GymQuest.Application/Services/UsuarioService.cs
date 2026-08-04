using Microsoft.AspNetCore.Identity;
using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UsuarioDto> RegistrarUsuarioAsync(CrearUsuarioDto dto)
    {
        var existente = await _usuarioRepository.ObtenerPorEmailAsync(dto.Email);
        if (existente is not null)
        {
            throw new InvalidOperationException("Ya existe un usuario registrado con ese email.");
        }

        var usuarioParaHash = new Usuario(dto.NombreUsuario, dto.Email, passwordHash: string.Empty);
        var hash = _passwordHasher.HashPassword(usuarioParaHash, dto.Password);

        var usuario = new Usuario(dto.NombreUsuario, dto.Email, hash);

        await _usuarioRepository.AgregarAsync(usuario);

        return new UsuarioDto(usuario.Id, usuario.NombreUsuario, usuario.Email, usuario.FechaRegistro);
    }

    public async Task<UsuarioDto> IniciarSesionAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(dto.Email);
        if (usuario is null)
        {
            throw new InvalidOperationException("Email o contraseña incorrectos.");
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password);
        if (resultado == PasswordVerificationResult.Failed)
        {
            throw new InvalidOperationException("Email o contraseña incorrectos.");
        }

        return new UsuarioDto(usuario.Id, usuario.NombreUsuario, usuario.Email, usuario.FechaRegistro);
    }
}