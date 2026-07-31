using GymQuest.Domain.Enums;

namespace GymQuest.Domain.Entities;

public class Mision
{
    public Guid Id { get; private set; }
    public Guid HeroeId { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;
    public TipoObjetivoMision TipoObjetivo { get; private set; }
    public int ValorObjetivo { get; private set; }
    public int ProgresoActual { get; private set; } = 0;
    public bool Completada { get; private set; } = false;
    public DateTime FechaExpiracion { get; private set; }

    protected Mision() { }

    public Mision(Guid heroeId, string descripcion, TipoObjetivoMision tipoObjetivo, int valorObjetivo, DateTime fechaExpiracion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción no puede estar vacía.", nameof(descripcion));

        if (valorObjetivo <= 0)
            throw new ArgumentException("El valor objetivo debe ser mayor a 0.", nameof(valorObjetivo));

        Id = Guid.NewGuid();
        HeroeId = heroeId;
        Descripcion = descripcion;
        TipoObjetivo = tipoObjetivo;
        ValorObjetivo = valorObjetivo;
        FechaExpiracion = fechaExpiracion;
    }

    /// <summary>
    /// Suma progreso a la misión. Si alcanza o supera el objetivo, se marca
    /// como completada automáticamente.
    /// </summary>
    public void RegistrarProgreso(int cantidad)
    {
        if (Completada)
            return; // ya completada, ignorar progreso adicional

        if (cantidad <= 0)
            throw new ArgumentException("El progreso a registrar debe ser mayor a 0.", nameof(cantidad));

        ProgresoActual = Math.Min(ProgresoActual + cantidad, ValorObjetivo);

        if (ProgresoActual >= ValorObjetivo)
            Completada = true;
    }
}