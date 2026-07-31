namespace GymQuest.Domain.Entities;

public class Logro
{
    public Guid Id { get; private set; }
    public Guid HeroeId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public bool Desbloqueado { get; private set; } = false;
    public DateTime? FechaDesbloqueo { get; private set; }

    protected Logro() { }

    public Logro(Guid heroeId, string nombre, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del logro no puede estar vacío.", nameof(nombre));

        Id = Guid.NewGuid();
        HeroeId = heroeId;
        Nombre = nombre;
        Descripcion = descripcion;
    }

    /// <summary>
    /// Desbloquea el logro. Si ya estaba desbloqueado, no hace nada
    /// (es una operación "idempotente": llamarla 2 veces no rompe nada).
    /// </summary>
    public void Desbloquear()
    {
        if (Desbloqueado)
            return;

        Desbloqueado = true;
        FechaDesbloqueo = DateTime.UtcNow;
    }
}