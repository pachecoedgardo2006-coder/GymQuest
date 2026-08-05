using FluentAssertions;
using GymQuest.Application.Services;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Enums;
using GymQuest.Domain.Interfaces;
using Moq;
using Xunit;

namespace GymQuest.Application.Tests;

public class MisionServiceTests
{
    private readonly Mock<IMisionRepository> _misionRepositoryMock = new();
    private readonly MisionService _sut;

    public MisionServiceTests()
    {
        _sut = new MisionService(_misionRepositoryMock.Object);
    }

    [Fact]
    public async Task RegistrarProgresoAsync_AlcanzaElObjetivo_MarcaComoCompletada()
    {
        // Arrange
        var mision = new Mision(
            Guid.NewGuid(),
            "Mantén 3 días de racha esta semana",
            TipoObjetivoMision.MantenerDiasDeRacha,
            valorObjetivo: 3,
            fechaExpiracion: DateTime.UtcNow.AddDays(7));

        _misionRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(mision.Id))
            .ReturnsAsync(mision);

        // Act
        var resultado = await _sut.RegistrarProgresoAsync(mision.Id, 3);

        // Assert
        resultado.Completada.Should().BeTrue();
        resultado.ProgresoActual.Should().Be(3);
        _misionRepositoryMock.Verify(r => r.ActualizarAsync(mision), Times.Once);
    }
}