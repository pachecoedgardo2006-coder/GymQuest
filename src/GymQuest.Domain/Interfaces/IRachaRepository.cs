using GymQuest.Domain.Entities;

namespace GymQuest.Domain.Interfaces;

public interface IRachaRepository
{
    Task<Racha?> ObtenerPorHeroeIdAsync(Guid heroeId);
    Task AgregarAsync(Racha racha);
    Task ActualizarAsync(Racha racha);
}