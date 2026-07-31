using GymQuest.Domain.Entities;
using Xunit;

namespace GymQuest.Domain.Tests;

public class RachaTests
{
    [Fact]
    public void RegistrarEntrenamientoHoy_PrimeraVez_IniciaRachaEn1()
    {
        var racha = new Racha(Guid.NewGuid(), diasProgramadosPorSemana: 4);

        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 1));

        Assert.Equal(1, racha.DiasConsecutivos);
    }

    [Fact]
    public void RegistrarEntrenamientoHoy_DiaConsecutivo_IncrementaRacha()
    {
        var racha = new Racha(Guid.NewGuid(), diasProgramadosPorSemana: 4);
        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 1));

        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 2));

        Assert.Equal(2, racha.DiasConsecutivos);
    }

    [Fact]
    public void RegistrarEntrenamientoHoy_SaltaUnDiaConPocionDisponible_ProtegeLaRacha()
    {
        var racha = new Racha(Guid.NewGuid(), diasProgramadosPorSemana: 4); // empieza con 1 poción
        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 1));

        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 3)); // se saltó el día 2

        Assert.Equal(2, racha.DiasConsecutivos); // se protegió, no se rompió
        Assert.Equal(0, racha.PocionesDeDescansoDisponibles); // consumió la poción
    }

    [Fact]
    public void RegistrarEntrenamientoHoy_SaltaVariosDiasSinPocion_RompeLaRacha()
    {
        var racha = new Racha(Guid.NewGuid(), diasProgramadosPorSemana: 4);
        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 1));
        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 3)); // usa la única poción aquí

        racha.RegistrarEntrenamientoHoy(new DateTime(2026, 7, 10)); // hueco grande, sin pociones

        Assert.Equal(1, racha.DiasConsecutivos); // se rompió, vuelve a empezar
    }

    [Fact]
    public void Constructor_ConDiasProgramadosFueraDeRango_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() => new Racha(Guid.NewGuid(), diasProgramadosPorSemana: 8));
    }
}