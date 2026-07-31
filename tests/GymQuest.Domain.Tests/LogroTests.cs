using GymQuest.Domain.Entities;
using Xunit;

namespace GymQuest.Domain.Tests;

public class LogroTests
{
    [Fact]
    public void Desbloquear_LogroNuevo_QuedaDesbloqueadoConFecha()
    {
        var logro = new Logro(Guid.NewGuid(), "Racha de Hierro", "10 sesiones seguidas");

        logro.Desbloquear();

        Assert.True(logro.Desbloqueado);
        Assert.NotNull(logro.FechaDesbloqueo);
    }

    [Fact]
    public void Desbloquear_LogroYaDesbloqueado_NoLanzaErrorYMantieneFecha()
    {
        var logro = new Logro(Guid.NewGuid(), "Racha de Hierro", "10 sesiones seguidas");
        logro.Desbloquear();
        var primeraFecha = logro.FechaDesbloqueo;

        logro.Desbloquear(); // segunda llamada, no debe romper nada

        Assert.Equal(primeraFecha, logro.FechaDesbloqueo);
    }
}