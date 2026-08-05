using FluentAssertions;
using GymQuest.Application.Services;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using Moq;
using Xunit;

namespace GymQuest.Application.Tests;

public class LogroServiceTests
{
    private readonly Mock<ILogroRepository> _logroRepositoryMock = new();
    private readonly LogroService _sut;

    public LogroServiceTests()
    {
        _sut = new LogroService(_logroRepositoryMock.Object);
    }

    [Fact]
    public async Task DesbloquearLogroAsync_LogroExistente_LoDesbloqueaYPersiste()
    {
        // Arrange
        var logro = new Logro(Guid.NewGuid(), "Primer Entrenamiento", "Completa tu primera sesión.");
        _logroRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(logro.Id))
            .ReturnsAsync(logro);

        // Act
        var resultado = await _sut.DesbloquearLogroAsync(logro.Id);

        // Assert
        resultado.Desbloqueado.Should().BeTrue();
        resultado.FechaDesbloqueo.Should().NotBeNull();
        _logroRepositoryMock.Verify(r => r.ActualizarAsync(logro), Times.Once);
    }

    [Fact]
    public async Task DesbloquearLogroAsync_LogroInexistente_LanzaInvalidOperationException()
    {
        // Arrange
        var logroId = Guid.NewGuid();
        _logroRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(logroId))
            .ReturnsAsync((Logro?)null);

        // Act
        var accion = async () => await _sut.DesbloquearLogroAsync(logroId);

        // Assert
        await accion.Should().ThrowAsync<InvalidOperationException>();
    }
}