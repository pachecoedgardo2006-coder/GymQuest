using GymQuest.Domain.Enums;

namespace GymQuest.Domain.Entities;

public class Ejercicio
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public CategoriaEjercicio Categoria { get; private set; }

    /// <summary>
    /// Multiplica el XP base ganado en este ejercicio.
    /// Ejercicios multiarticulares (sentadilla, peso muerto) suelen tener un
    /// factor más alto porque implican más esfuerzo global.
    /// </summary>
    public decimal FactorXp { get; private set; }

    protected Ejercicio() { }

    public Ejercicio(string nombre, CategoriaEjercicio categoria, decimal factorXp)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del ejercicio no puede estar vacío.", nameof(nombre));

        if (factorXp <= 0)
            throw new ArgumentException("El factor de XP debe ser mayor a 0.", nameof(factorXp));

        Id = Guid.NewGuid();
        Nombre = nombre;
        Categoria = categoria;
        FactorXp = factorXp;
    }
}