namespace GymQuest.Domain.Entities;

public class SerieRealizada
{
    public Guid Id { get; private set; }
    public Guid EjercicioId { get; private set; }
    public decimal PesoLevantado { get; private set; }
    public int RepeticionesRealizadas { get; private set; }
    public bool EsRecordPersonal { get; private set; }

    protected SerieRealizada() { }

    public SerieRealizada(Guid ejercicioId, decimal pesoLevantado, int repeticionesRealizadas, bool esRecordPersonal = false)
    {
        if (pesoLevantado < 0)
            throw new ArgumentException("El peso no puede ser negativo.", nameof(pesoLevantado));

        if (repeticionesRealizadas <= 0)
            throw new ArgumentException("Las repeticiones deben ser mayores a 0.", nameof(repeticionesRealizadas));

        Id = Guid.NewGuid();
        EjercicioId = ejercicioId;
        PesoLevantado = pesoLevantado;
        RepeticionesRealizadas = repeticionesRealizadas;
        EsRecordPersonal = esRecordPersonal;
    }

    /// <summary>
    /// Calcula el XP de esta serie según el algoritmo del documento:
    /// Peso x Repeticiones x Factor de Ejercicio.
    /// Si es récord personal, aplica un bonus del 20%.
    /// </summary>
    public int CalcularXp(decimal factorEjercicio)
    {
        var xpBase = PesoLevantado * RepeticionesRealizadas * factorEjercicio;

        if (EsRecordPersonal)
            xpBase *= 1.2m;

        return (int)Math.Round(xpBase, MidpointRounding.AwayFromZero);
    }
}