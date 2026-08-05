using FluentAssertions;
using GymQuest.Application.DTOs;
using GymQuest.Application.Services;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using Moq;
using Xunit;

namespace GymQuest.Application.Tests;

public class RutinaServiceTests
{
    private readonly Mock<IRutinaRepository> _rutinaRepositoryMock = new();
    private readonly RutinaService _sut; // "sut" = System Under Test

    public RutinaServiceTests()
    {
        _sut = new RutinaService(_rutinaRepositoryMock.Object);
    }

    [Fact]
    public async Task CrearRutinaAsync_ConEjerciciosValidos_CreaLaRutinaYLaPersiste()
    {
        // Arrange
        var dto = new CrearRutinaDto(
            Guid.NewGuid(),
            "Rutina de Fuerza",
            new List<CrearRutinaEjercicioDto>
            {
                new(Guid.NewGuid(), 4, 8, 60m)
            },
            false // EsPlantillaPredeterminada (opcional)
        );

        // Act
        var resultado = await _sut.CrearRutinaAsync(dto);

        // Assert
        resultado.Nombre.Should().Be("Rutina de Fuerza");
        resultado.Ejercicios.Should().HaveCount(1);
        _rutinaRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Rutina>()), Times.Once);
    }

    [Fact]
    public async Task CrearRutinaAsync_SinEjercicios_LanzaArgumentException()
    {
        // Arrange
        var dto = new CrearRutinaDto(Guid.NewGuid(), "Rutina Vacía", new List<CrearRutinaEjercicioDto>());

        // Act
        var accion = async () => await _sut.CrearRutinaAsync(dto);

        // Assert
        await accion.Should().ThrowAsync<ArgumentException>();
        _rutinaRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<Rutina>()), Times.Never);
    }
}