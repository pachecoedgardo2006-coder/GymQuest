using FluentAssertions;
using GymQuest.Application.Services;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using Moq;
using Xunit;

namespace GymQuest.Application.Tests;

public class RachaServiceTests
{
    private readonly Mock<IRachaRepository> _rachaRepositoryMock = new();
    private readonly RachaService _sut;

    public RachaServiceTests()
    {
        _sut = new RachaService(_rachaRepositoryMock.Object);
    }

    [Fact]
    public async Task RegistrarEntrenamientoDeHoyAsync_SinRachaPrevia_CreaUnaNuevaYLaAgrega()
    {
        // Arrange
        var heroeId = Guid.NewGuid();
        _rachaRepositoryMock
            .Setup(r => r.ObtenerPorHeroeIdAsync(heroeId))
            .ReturnsAsync((Racha?)null);

        // Act
        var resultado = await _sut.RegistrarEntrenamientoDeHoyAsync(heroeId, DateTime.UtcNow);

        // Assert
        resultado.DiasConsecutivos.Should().Be(1);
        _rachaRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Racha>()), Times.Once);
        _rachaRepositoryMock.Verify(r => r.ActualizarAsync(It.IsAny<Racha>()), Times.Never);
    }

    [Fact]
    public async Task RegistrarEntrenamientoDeHoyAsync_ConRachaExistente_LaActualiza()
    {
        // Arrange
        var heroeId = Guid.NewGuid();
        var racha = new Racha(heroeId, 3);
        racha.RegistrarEntrenamientoHoy(DateTime.UtcNow.AddDays(-1));

        _rachaRepositoryMock
            .Setup(r => r.ObtenerPorHeroeIdAsync(heroeId))
            .ReturnsAsync(racha);

        // Act
        var resultado = await _sut.RegistrarEntrenamientoDeHoyAsync(heroeId, DateTime.UtcNow);

        // Assert
        resultado.DiasConsecutivos.Should().Be(2);
        _rachaRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Racha>()), Times.Never);
        _rachaRepositoryMock.Verify(r => r.ActualizarAsync(racha), Times.Once);
    }

    [Fact]
    public async Task ObtenerRachaAsync_SiNoExisteRacha_LanzaInvalidOperationException()
    {
        // Arrange
        var heroeId = Guid.NewGuid();
        _rachaRepositoryMock
            .Setup(r => r.ObtenerPorHeroeIdAsync(heroeId))
            .ReturnsAsync((Racha?)null);

        // Act
        var accion = async () => await _sut.ObtenerRachaAsync(heroeId);

        // Assert
        await accion.Should().ThrowAsync<InvalidOperationException>();
    }
}