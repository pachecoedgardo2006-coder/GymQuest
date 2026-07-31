using GymQuest.Domain.Entities;
using Xunit;

namespace GymQuest.Domain.Tests;

public class HeroeTests
{
    [Fact]
    public void GanarExperiencia_SumaXpSinSuperarUmbral_NoSubeDeNivel()
    {
        var heroe = new Heroe(Guid.NewGuid());

        heroe.GanarExperiencia(50); // umbral inicial es 100

        Assert.Equal(1, heroe.Nivel);
        Assert.Equal(50, heroe.ExperienciaActual);
    }

    [Fact]
    public void GanarExperiencia_SuperaUmbral_SubeDeNivelYConservaExcedente()
    {
        var heroe = new Heroe(Guid.NewGuid());

        heroe.GanarExperiencia(120); // umbral inicial es 100

        Assert.Equal(2, heroe.Nivel);
        Assert.Equal(20, heroe.ExperienciaActual); // 120 - 100 = 20 de excedente
    }

    [Fact]
    public void GanarExperiencia_ConValorNegativoOCero_LanzaExcepcion()
    {
        var heroe = new Heroe(Guid.NewGuid());

        Assert.Throws<ArgumentException>(() => heroe.GanarExperiencia(0));
    }
}