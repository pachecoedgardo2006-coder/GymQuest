using Microsoft.AspNetCore.Identity;
using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHeroeRepository _heroeRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IHeroeRepository heroeRepository,
        IPasswordHasher<Usuario> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _usuarioRepository = usuarioRepository;
        _heroeRepository = heroeRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegistrarUsuarioAsync(CrearUsuarioDto dto)
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

        // Cada Usuario tiene exactamente un Heroe (su avatar), y nace junto
        // con él: nivel 1, XP 0, título "Novato de Hierro" (valores por
        // defecto de la entidad Heroe). Así el usuario nunca ve un estado
        // "sin héroe" tras registrarse.
        var heroe = new Heroe(usuario.Id);
        await _heroeRepository.AgregarAsync(heroe);

        return ConstruirRespuestaAuth(usuario);
    }

    public async Task<AuthResponseDto> IniciarSesionAsync(LoginDto dto)
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

        return ConstruirRespuestaAuth(usuario);
    }

    private AuthResponseDto ConstruirRespuestaAuth(Usuario usuario)
    {
        var token = _jwtTokenGenerator.GenerarToken(usuario);
        var usuarioDto = new UsuarioDto(usuario.Id, usuario.NombreUsuario, usuario.Email, usuario.FechaRegistro);

        return new AuthResponseDto(usuarioDto, token);
    }
}