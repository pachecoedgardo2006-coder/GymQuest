using GymQuest.Domain.Entities;

namespace GymQuest.Domain.Interfaces;

public interface IMisionRepository
{
    Task<Mision?> ObtenerPorIdAsync(Guid id);
    Task<List<Mision>> ObtenerActivasPorHeroeIdAsync(Guid heroeId);
    Task AgregarAsync(Mision mision);
    Task ActualizarAsync(Mision mision);
}