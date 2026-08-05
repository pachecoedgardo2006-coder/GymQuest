using GymQuest.Domain.Entities;
public interface ILogroRepository
{
    Task<Logro?> ObtenerPorIdAsync(Guid id);
    Task<List<Logro>> ObtenerPorHeroeIdAsync(Guid heroeId);
    Task AgregarAsync(Logro logro);
    Task ActualizarAsync(Logro logro);
}