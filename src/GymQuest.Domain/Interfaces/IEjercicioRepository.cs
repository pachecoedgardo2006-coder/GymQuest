using GymQuest.Domain.Entities;

namespace GymQuest.Domain.Interfaces;

public interface IEjercicioRepository
{
    Task<Ejercicio?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<Ejercicio>> ObtenerTodosAsync();
    Task<IEnumerable<Ejercicio>> ObtenerPorIdsAsync(IEnumerable<Guid> ids);
    Task AgregarAsync(Ejercicio ejercicio);
}