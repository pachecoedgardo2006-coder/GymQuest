using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class RutinaRepository : IRutinaRepository
{
    private readonly GymQuestDbContext _context;

    public RutinaRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<Rutina?> ObtenerPorIdAsync(Guid id) =>
        await _context.Rutinas
            .Include(r => r.Ejercicios)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Rutina>> ObtenerPorUsuarioIdAsync(Guid usuarioId) =>
        await _context.Rutinas
            .Include(r => r.Ejercicios)
            .Where(r => r.UsuarioId == usuarioId)
            .ToListAsync();

    public async Task<IEnumerable<Rutina>> ObtenerPlantillasPredeterminadasAsync() =>
        await _context.Rutinas
            .Include(r => r.Ejercicios)
            .Where(r => r.EsPlantillaPredeterminada)
            .ToListAsync();

    public async Task AgregarAsync(Rutina rutina)
    {
        // Al agregar la Rutina, EF Core detecta y agrega en cascada
        // los RutinaEjercicio que ya estén en rutina.Ejercicios
        // (gracias a la relación configurada en RutinaConfiguration).
        await _context.Rutinas.AddAsync(rutina);
        await _context.SaveChangesAsync();
    }
}