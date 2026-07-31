using GymQuest.Domain.Entities;
using Xunit;

namespace GymQuest.Domain.Tests;

public class SesionEntrenamientoTests
{
    [Fact]
    public void Finalizar_ConSeriesRegistradas_CalculaXpTotalIncluyendoBase()
    {
        var ejercicioId = Guid.NewGuid();
        var sesion = new SesionEntrenamiento(Guid.NewGuid(), Guid.NewGuid());
        sesion.RegistrarSerie(new SerieRealizada(ejercicioId, pesoLevantado: 20, repeticionesRealizadas: 10));

        var factores = new Dictionary<Guid, decimal> { [ejercicioId] = 1.0m };

        // XP base rutina (20) + serie (20x10x1.0 = 200) = 220
        var xpTotal = sesion.Finalizar(factores);

        Assert.Equal(220, xpTotal);
        Assert.True(sesion.Completada);
    }

    [Fact]
    public void Finalizar_SinSeriesRegistradas_LanzaExcepcion()
    {
        var sesion = new SesionEntrenamiento(Guid.NewGuid(), Guid.NewGuid());
        var factores = new Dictionary<Guid, decimal>();

        Assert.Throws<InvalidOperationException>(() => sesion.Finalizar(factores));
    }

    [Fact]
    public void Finalizar_SesionYaFinalizada_LanzaExcepcion()
    {
        var ejercicioId = Guid.NewGuid();
        var sesion = new SesionEntrenamiento(Guid.NewGuid(), Guid.NewGuid());
        sesion.RegistrarSerie(new SerieRealizada(ejercicioId, 20, 10));
        var factores = new Dictionary<Guid, decimal> { [ejercicioId] = 1.0m };
        sesion.Finalizar(factores); // primera vez, OK

        Assert.Throws<InvalidOperationException>(() => sesion.Finalizar(factores)); // segunda vez, debe fallar
    }

    [Fact]
    public void RegistrarSerie_EnSesionYaCompletada_LanzaExcepcion()
    {
        var ejercicioId = Guid.NewGuid();
        var sesion = new SesionEntrenamiento(Guid.NewGuid(), Guid.NewGuid());
        sesion.RegistrarSerie(new SerieRealizada(ejercicioId, 20, 10));
        sesion.Finalizar(new Dictionary<Guid, decimal> { [ejercicioId] = 1.0m });

        Assert.Throws<InvalidOperationException>(() =>
            sesion.RegistrarSerie(new SerieRealizada(ejercicioId, 20, 10)));
    }
}