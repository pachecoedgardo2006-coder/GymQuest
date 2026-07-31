using GymQuest.Domain.Entities;

namespace GymQuest.Domain.Interfaces;

public interface IHeroeRepository
{
    Task<Heroe?> ObtenerPorIdAsync(Guid id);
    Task<Heroe?> ObtenerPorUsuarioIdAsync(Guid usuarioId);
    Task AgregarAsync(Heroe heroe);
    Task ActualizarAsync(Heroe heroe);
}