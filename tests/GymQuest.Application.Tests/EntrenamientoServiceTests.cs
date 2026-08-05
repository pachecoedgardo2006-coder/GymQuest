using FluentAssertions;
using GymQuest.Application.DTOs;
using GymQuest.Application.Services;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Enums;
using GymQuest.Domain.Interfaces;
using Moq;
using Xunit;

namespace GymQuest.Application.Tests;

public class EntrenamientoServiceTests
{
    private readonly Mock<ISesionEntrenamientoRepository> _sesionRepositoryMock = new();
    private readonly Mock<IEjercicioRepository> _ejercicioRepositoryMock = new();
    private readonly Mock<IHeroeRepository> _heroeRepositoryMock = new();
    private readonly EntrenamientoService _sut;

    public EntrenamientoServiceTests()
    {
        _sut = new EntrenamientoService(
            _sesionRepositoryMock.Object,
            _ejercicioRepositoryMock.Object,
            _heroeRepositoryMock.Object);
    }

    [Fact]
    public async Task IniciarEntrenamientoAsync_CreaLaSesionYLaPersiste()
    {
        // Arrange
        var dto = new IniciarEntrenamientoDto(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var sesionId = await _sut.IniciarEntrenamientoAsync(dto);

        // Assert
        sesionId.Should().NotBeEmpty();
        _sesionRepositoryMock.Verify(r => r.AgregarAsync(It.IsAny<SesionEntrenamiento>()), Times.Once);
    }

    [Fact]
    public async Task RegistrarSerieAsync_SesionExiste_AgregaLaSerie()
    {
        // Arrange
        var sesion = new SesionEntrenamiento(Guid.NewGuid(), Guid.NewGuid());
        var dto = new RegistrarSerieDto(sesion.Id, Guid.NewGuid(), 80m, 10, false);

        _sesionRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(sesion.Id))
            .ReturnsAsync(sesion);

        // Act
        await _sut.RegistrarSerieAsync(dto);

        // Assert
        sesion.Series.Should().HaveCount(1);
    }

    [Fact]
    public async Task RegistrarSerieAsync_SesionNoExiste_LanzaInvalidOperationException()
    {
        // Arrange
        var dto = new RegistrarSerieDto(Guid.NewGuid(), Guid.NewGuid(), 80m, 10, false);

        _sesionRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(dto.SesionId))
            .ReturnsAsync((SesionEntrenamiento?)null);

        // Act
        var accion = async () => await _sut.RegistrarSerieAsync(dto);

        // Assert
        await accion.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task FinalizarEntrenamientoAsync_CalculaXpYLaSumaAlHeroe_DetectaSubidaDeNivel()
    {
        // Arrange
        var heroe = new Heroe(Guid.NewGuid()); // Nivel 1, 0 XP, necesita 100 para subir
        var ejercicioId = Guid.NewGuid();

        var sesion = new SesionEntrenamiento(heroe.Id, Guid.NewGuid());
        // Peso 100 x Reps 10 x Factor 1.0 = 1000 XP de la serie + 20 base = 1020 XP
        // Con 1020 XP, el héroe sube de nivel 1 a nivel 2 (y probablemente más)
        sesion.RegistrarSerie(new SerieRealizada(ejercicioId, 100m, 10));

        var ejercicio = new Ejercicio("Sentadilla", CategoriaEjercicio.Multiarticular, 1.0m);

        _sesionRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(sesion.Id))
            .ReturnsAsync(sesion);

        _ejercicioRepositoryMock
            .Setup(r => r.ObtenerPorIdsAsync(It.Is<IEnumerable<Guid>>(ids => ids.Contains(ejercicioId))))
            .ReturnsAsync(new List<Ejercicio> { ejercicio });

        _heroeRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(heroe.Id))
            .ReturnsAsync(heroe);

        // Act
        var resultado = await _sut.FinalizarEntrenamientoAsync(sesion.Id);

        // Assert
        resultado.NivelAnterior.Should().Be(1);
        resultado.NivelActual.Should().BeGreaterThan(1);
        resultado.SubioDeNivel.Should().BeTrue();
        _heroeRepositoryMock.Verify(r => r.ActualizarAsync(heroe), Times.Once);
    }

    [Fact]
    public async Task FinalizarEntrenamientoAsync_SesionSinSeries_LanzaInvalidOperationException()
    {
        // Arrange
        var sesion = new SesionEntrenamiento(Guid.NewGuid(), Guid.NewGuid());

        _sesionRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(sesion.Id))
            .ReturnsAsync(sesion);

        // Act
        var accion = async () => await _sut.FinalizarEntrenamientoAsync(sesion.Id);

        // Assert — la excepción viene de sesion.Finalizar() dentro del servicio
        await accion.Should().ThrowAsync<InvalidOperationException>();
    }
}