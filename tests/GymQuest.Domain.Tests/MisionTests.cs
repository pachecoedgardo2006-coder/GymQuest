using GymQuest.Domain.Entities;
using GymQuest.Domain.Enums;
using Xunit;

namespace GymQuest.Domain.Tests;

public class MisionTests
{
    [Fact]
    public void RegistrarProgreso_NoAlcanzaObjetivo_QuedaIncompleta()
    {
        var mision = new Mision(Guid.NewGuid(), "Mantén 3 días de racha", TipoObjetivoMision.MantenerDiasDeRacha, valorObjetivo: 3, DateTime.UtcNow.AddDays(7));

        mision.RegistrarProgreso(2);

        Assert.False(mision.Completada);
        Assert.Equal(2, mision.ProgresoActual);
    }

    [Fact]
    public void RegistrarProgreso_AlcanzaObjetivo_SeMarcaCompletada()
    {
        var mision = new Mision(Guid.NewGuid(), "Mantén 3 días de racha", TipoObjetivoMision.MantenerDiasDeRacha, valorObjetivo: 3, DateTime.UtcNow.AddDays(7));

        mision.RegistrarProgreso(3);

        Assert.True(mision.Completada);
    }

    [Fact]
    public void RegistrarProgreso_SuperaObjetivo_NoQuedaPorEncimaDelValorObjetivo()
    {
        var mision = new Mision(Guid.NewGuid(), "Mantén 3 días de racha", TipoObjetivoMision.MantenerDiasDeRacha, valorObjetivo: 3, DateTime.UtcNow.AddDays(7));

        mision.RegistrarProgreso(10);

        Assert.Equal(3, mision.ProgresoActual); // no se pasa de 3, aunque le dimos 10
        Assert.True(mision.Completada);
    }

    [Fact]
    public void Constructor_ConValorObjetivoCero_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            new Mision(Guid.NewGuid(), "Test", TipoObjetivoMision.AumentarPesoEnEjercicio, valorObjetivo: 0, DateTime.UtcNow));
    }
}