namespace GymQuest.Domain.Entities;

public class RutinaEjercicio
{
    public Guid Id { get; private set; }
    public Guid RutinaId { get; private set; }
    public Guid EjercicioId { get; private set; }
    public int SeriesObjetivo { get; private set; }
    public int RepeticionesObjetivo { get; private set; }
    public decimal PesoInicial { get; private set; }

    protected RutinaEjercicio() { }

    public RutinaEjercicio(Guid rutinaId, Guid ejercicioId, int seriesObjetivo, int repeticionesObjetivo, decimal pesoInicial)
    {
        if (seriesObjetivo <= 0)
            throw new ArgumentException("Las series objetivo deben ser mayores a 0.", nameof(seriesObjetivo));

        if (repeticionesObjetivo <= 0)
            throw new ArgumentException("Las repeticiones objetivo deben ser mayores a 0.", nameof(repeticionesObjetivo));

        Id = Guid.NewGuid();
        RutinaId = rutinaId;
        EjercicioId = ejercicioId;
        SeriesObjetivo = seriesObjetivo;
        RepeticionesObjetivo = repeticionesObjetivo;
        PesoInicial = pesoInicial;
    }
}