namespace GymQuest.Domain.Entities;

public class SesionEntrenamiento
{
    public Guid Id { get; private set; }
    public Guid HeroeId { get; private set; }
    public Guid RutinaId { get; private set; }
    public DateTime Fecha { get; private set; }
    public bool Completada { get; private set; }

    private readonly List<SerieRealizada> _series = new();
    public IReadOnlyCollection<SerieRealizada> Series => _series.AsReadOnly();

    private const int XpBasePorRutinaCompleta = 20;

    protected SesionEntrenamiento() { }

    public SesionEntrenamiento(Guid heroeId, Guid rutinaId)
    {
        Id = Guid.NewGuid();
        HeroeId = heroeId;
        RutinaId = rutinaId;
        Fecha = DateTime.UtcNow;
        Completada = false;
    }

    public void RegistrarSerie(SerieRealizada serie)
    {
        if (Completada)
            throw new InvalidOperationException("No se pueden agregar series a una sesión ya finalizada.");

        _series.Add(serie);
    }

    /// <summary>
    /// Marca la sesión como completada y calcula el XP total ganado,
    /// según el diccionario de factores por ejercicio.
    /// </summary>
    public int Finalizar(IReadOnlyDictionary<Guid, decimal> factoresPorEjercicio)
    {
        if (Completada)
            throw new InvalidOperationException("Esta sesión ya fue finalizada.");

        if (_series.Count == 0)
            throw new InvalidOperationException("No se puede finalizar una sesión sin series registradas.");

        var xpTotal = XpBasePorRutinaCompleta;

        foreach (var serie in _series)
        {
            var factor = factoresPorEjercicio.TryGetValue(serie.EjercicioId, out var f) ? f : 1.0m;
            xpTotal += serie.CalcularXp(factor);
        }

        Completada = true;
        return xpTotal;
    }
}