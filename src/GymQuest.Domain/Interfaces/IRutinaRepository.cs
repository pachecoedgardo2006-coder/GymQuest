using GymQuest.Domain.Entities;

namespace GymQuest.Domain.Interfaces;

public interface IRutinaRepository
{
    Task<Rutina?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<Rutina>> ObtenerPorUsuarioIdAsync(Guid usuarioId);
    Task<IEnumerable<Rutina>> ObtenerPlantillasPredeterminadasAsync();
    Task AgregarAsync(Rutina rutina);
}