using GymQuest.Domain.Entities;

namespace GymQuest.Domain.Interfaces;

public interface ISesionEntrenamientoRepository
{
    Task<SesionEntrenamiento?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<SesionEntrenamiento>> ObtenerPorHeroeIdAsync(Guid heroeId);
    Task AgregarAsync(SesionEntrenamiento sesion);
}