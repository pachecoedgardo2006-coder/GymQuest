namespace GymQuest.Domain.Entities;

public class Racha
{
    public Guid Id { get; private set; }
    public Guid HeroeId { get; private set; }
    public int DiasConsecutivos { get; private set; } = 0;
    public int DiasProgramadosPorSemana { get; private set; }
    public int PocionesDeDescansoDisponibles { get; private set; } = 1;
    public DateTime? UltimaFechaEntrenada { get; private set; }

    private const int MaxDiasSinEntrenarConProteccion = 1;

    protected Racha() { }

    public Racha(Guid heroeId, int diasProgramadosPorSemana)
    {
        if (diasProgramadosPorSemana is < 1 or > 7)
            throw new ArgumentException("Los días programados deben estar entre 1 y 7.", nameof(diasProgramadosPorSemana));

        Id = Guid.NewGuid();
        HeroeId = heroeId;
        DiasProgramadosPorSemana = diasProgramadosPorSemana;
    }

    /// <summary>
    /// Registra que el usuario entrenó hoy. Si venía de un hueco de días
    /// sin entrenar, decide si la racha se rompe o si se protege con una poción.
    /// </summary>
    public void RegistrarEntrenamientoHoy(DateTime fechaHoy)
    {
        if (UltimaFechaEntrenada is null)
        {
            DiasConsecutivos = 1;
            UltimaFechaEntrenada = fechaHoy.Date;
            return;
        }

        var diasSinEntrenar = (fechaHoy.Date - UltimaFechaEntrenada.Value.Date).Days;

        switch (diasSinEntrenar)
        {
            case 0:
                // Ya entrenó hoy, no hace nada (evita contar dos veces el mismo día)
                return;

            case 1:
                // Día consecutivo normal
                DiasConsecutivos++;
                break;

            case > 1 when PocionesDeDescansoDisponibles > 0 && diasSinEntrenar - 1 <= MaxDiasSinEntrenarConProteccion:
                // Se saltó días, pero tiene poción disponible: protege la racha
                PocionesDeDescansoDisponibles--;
                DiasConsecutivos++;
                break;

            default:
                // Se rompió la racha, empieza de cero
                DiasConsecutivos = 1;
                break;
        }

        UltimaFechaEntrenada = fechaHoy.Date;
    }

    public void OtorgarPocionDeDescanso()
    {
        PocionesDeDescansoDisponibles++;
    }
}