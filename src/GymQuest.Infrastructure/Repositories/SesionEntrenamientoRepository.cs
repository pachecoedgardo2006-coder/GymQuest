using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class SesionEntrenamientoRepository : ISesionEntrenamientoRepository
{
    private readonly GymQuestDbContext _context;

    public SesionEntrenamientoRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<SesionEntrenamiento?> ObtenerPorIdAsync(Guid id) =>
        await _context.SesionesEntrenamiento
            .Include(s => s.Series)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<SesionEntrenamiento>> ObtenerPorHeroeIdAsync(Guid heroeId) =>
        await _context.SesionesEntrenamiento
            .Include(s => s.Series)
            .Where(s => s.HeroeId == heroeId)
            .ToListAsync();

    public async Task AgregarAsync(SesionEntrenamiento sesion)
    {
        await _context.SesionesEntrenamiento.AddAsync(sesion);
        await _context.SaveChangesAsync();
    }
}