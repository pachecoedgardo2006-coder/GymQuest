using GymQuest.Domain.Entities;
using Xunit;

namespace GymQuest.Domain.Tests;

public class SerieRealizadaTests
{
    [Fact]
    public void CalcularXp_SinRecordPersonal_CalculaXpBase()
    {
        var serie = new SerieRealizada(Guid.NewGuid(), pesoLevantado: 50, repeticionesRealizadas: 10);

        // 50 (peso) x 10 (reps) x 1.5 (factor) = 750
        var xp = serie.CalcularXp(factorEjercicio: 1.5m);

        Assert.Equal(750, xp);
    }

    [Fact]
    public void CalcularXp_ConRecordPersonal_AplicaBonusDel20Porciento()
    {
        var serie = new SerieRealizada(Guid.NewGuid(), pesoLevantado: 50, repeticionesRealizadas: 10, esRecordPersonal: true);

        // 750 base x 1.2 (bonus PR) = 900
        var xp = serie.CalcularXp(factorEjercicio: 1.5m);

        Assert.Equal(900, xp);
    }

    [Fact]
    public void Constructor_ConRepeticionesCero_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            new SerieRealizada(Guid.NewGuid(), pesoLevantado: 50, repeticionesRealizadas: 0));
    }
}