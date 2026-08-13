using FluentAssertions;
using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Application.Services;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace GymQuest.Application.Tests;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
    private readonly Mock<IHeroeRepository> _heroeRepositoryMock = new();
    private readonly Mock<IPasswordHasher<Usuario>> _passwordHasherMock = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();
    private readonly UsuarioService _sut;

    public UsuarioServiceTests()
    {
        _jwtTokenGeneratorMock
            .Setup(j => j.GenerarToken(It.IsAny<Usuario>()))
            .Returns("token-simulado");

        _sut = new UsuarioService(
            _usuarioRepositoryMock.Object,
            _heroeRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task RegistrarUsuarioAsync_EmailNoRegistrado_CreaElUsuario()
    {
        // Arrange
        var dto = new CrearUsuarioDto("Carlos", "carlos@test.com", "Password123");

        _usuarioRepositoryMock
            .Setup(r => r.ObtenerPorEmailAsync(dto.Email))
            .ReturnsAsync((Usuario?)null);

        _passwordHasherMock
            .Setup(h => h.HashPassword(It.IsAny<Usuario>(), dto.Password))
            .Returns("hash-simulado");

        // Act
        var resultado = await _sut.RegistrarUsuarioAsync(dto);

        // Assert
        resultado.Usuario.NombreUsuario.Should().Be("Carlos");
        resultado.Usuario.Email.Should().Be("carlos@test.com");
        resultado.Token.Should().Be("token-simulado");
        _usuarioRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Once);
        _heroeRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Heroe>()), Times.Once);
    }

    [Fact]
    public async Task RegistrarUsuarioAsync_EmailYaRegistrado_LanzaInvalidOperationException()
    {
        // Arrange
        var dto = new CrearUsuarioDto("Carlos", "carlos@test.com", "Password123");
        var usuarioExistente = new Usuario("Carlos", "carlos@test.com", "hash-existente");

        _usuarioRepositoryMock
            .Setup(r => r.ObtenerPorEmailAsync(dto.Email))
            .ReturnsAsync(usuarioExistente);

        // Act
        var accion = async () => await _sut.RegistrarUsuarioAsync(dto);

        // Assert
        await accion.Should().ThrowAsync<InvalidOperationException>();
        _usuarioRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Never);
        _heroeRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Heroe>()), Times.Never);
    }

    [Fact]
    public async Task IniciarSesionAsync_CredencialesCorrectas_DevuelveAuthResponseDto()
    {
        // Arrange
        var usuario = new Usuario("Carlos", "carlos@test.com", "hash-real");
        var dto = new LoginDto("carlos@test.com", "Password123");

        _usuarioRepositoryMock
            .Setup(r => r.ObtenerPorEmailAsync(dto.Email))
            .ReturnsAsync(usuario);

        _passwordHasherMock
            .Setup(h => h.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password))
            .Returns(PasswordVerificationResult.Success);

        // Act
        var resultado = await _sut.IniciarSesionAsync(dto);

        // Assert
        resultado.Usuario.Email.Should().Be("carlos@test.com");
        resultado.Token.Should().Be("token-simulado");
    }

    [Fact]
    public async Task IniciarSesionAsync_EmailNoExiste_LanzaInvalidOperationExceptionConMensajeGenerico()
    {
        // Arrange
        var dto = new LoginDto("noexiste@test.com", "Password123");

        _usuarioRepositoryMock
            .Setup(r => r.ObtenerPorEmailAsync(dto.Email))
            .ReturnsAsync((Usuario?)null);

        // Act
        var accion = async () => await _sut.IniciarSesionAsync(dto);

        // Assert — mismo mensaje que password incorrecto, para no filtrar si el email existe
        await accion.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email o contraseña incorrectos.");
    }

    [Fact]
    public async Task IniciarSesionAsync_PasswordIncorrecto_LanzaInvalidOperationExceptionConMensajeGenerico()
    {
        // Arrange
        var usuario = new Usuario("Carlos", "carlos@test.com", "hash-real");
        var dto = new LoginDto("carlos@test.com", "PasswordIncorrecto");

        _usuarioRepositoryMock
            .Setup(r => r.ObtenerPorEmailAsync(dto.Email))
            .ReturnsAsync(usuario);

        _passwordHasherMock
            .Setup(h => h.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        var accion = async () => await _sut.IniciarSesionAsync(dto);

        // Assert
        await accion.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email o contraseña incorrectos.");
    }
}