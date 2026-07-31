namespace GymQuest.Domain.Entities;

public class Heroe
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string Titulo { get; private set; } = "Novato de Hierro";
    public int Nivel { get; private set; } = 1;
    public int ExperienciaActual { get; private set; } = 0;
    public int ExperienciaParaSiguienteNivel { get; private set; } = 100;

    public int Fuerza { get; private set; } = 0;
    public int Resistencia { get; private set; } = 0;
    public int Consistencia { get; private set; } = 0;

    protected Heroe() { }

    public Heroe(Guid usuarioId)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
    }

    /// <summary>
    /// Agrega XP al héroe. Si se alcanza el umbral, sube de nivel
    /// automáticamente (y puede subir varios niveles de una vez si la
    /// ganancia de XP es muy grande).
    /// </summary>
    public void GanarExperiencia(int xpGanada)
    {
        if (xpGanada <= 0)
            throw new ArgumentException("La experiencia ganada debe ser mayor a 0.", nameof(xpGanada));

        ExperienciaActual += xpGanada;

        while (ExperienciaActual >= ExperienciaParaSiguienteNivel)
        {
            ExperienciaActual -= ExperienciaParaSiguienteNivel;
            SubirNivel();
        }
    }

    private void SubirNivel()
    {
        Nivel++;
        // Cada nivel exige un poco más de XP que el anterior (curva de progresión simple).
        ExperienciaParaSiguienteNivel = (int)(ExperienciaParaSiguienteNivel * 1.15);
        ActualizarTitulo();
    }

    private void ActualizarTitulo()
    {
        Titulo = Nivel switch
        {
            < 5 => "Novato de Hierro",
            < 10 => "Guerrero en Ascenso",
            < 20 => "Veterano de Acero",
            _ => "Leyenda del Gimnasio"
        };
    }

    public void AumentarFuerza(int puntos) => Fuerza += puntos;
    public void AumentarResistencia(int puntos) => Resistencia += puntos;
    public void AumentarConsistencia(int puntos) => Consistencia += puntos;
}