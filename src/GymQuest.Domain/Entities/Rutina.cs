namespace GymQuest.Domain.Entities;

public class Rutina
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public bool EsPlantillaPredeterminada { get; private set; }

    private readonly List<RutinaEjercicio> _ejercicios = new();
    public IReadOnlyCollection<RutinaEjercicio> Ejercicios => _ejercicios.AsReadOnly();

    protected Rutina() { }

    public Rutina(Guid usuarioId, string nombre, bool esPlantillaPredeterminada = false)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la rutina no puede estar vacío.", nameof(nombre));

        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        Nombre = nombre;
        EsPlantillaPredeterminada = esPlantillaPredeterminada;
    }

    public void AgregarEjercicio(RutinaEjercicio ejercicio)
    {
        _ejercicios.Add(ejercicio);
    }
}