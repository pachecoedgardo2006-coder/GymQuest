using FluentAssertions;
using GymQuest.Application.DTOs;
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
    private readonly Mock<IPasswordHasher<Usuario>> _passwordHasherMock = new();
    private readonly UsuarioService _sut;

    public UsuarioServiceTests()
    {
        _sut = new UsuarioService(_usuarioRepositoryMock.Object, _passwordHasherMock.Object);
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
        resultado.NombreUsuario.Should().Be("Carlos");
        resultado.Email.Should().Be("carlos@test.com");
        _usuarioRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Once);
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
    }

    [Fact]
    public async Task IniciarSesionAsync_CredencialesCorrectas_DevuelveUsuarioDto()
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
        resultado.Email.Should().Be("carlos@test.com");
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