using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class MisionRepository : IMisionRepository
{
    private readonly GymQuestDbContext _context;

    public MisionRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<Mision?> ObtenerPorIdAsync(Guid id) =>
        await _context.Misiones.FirstOrDefaultAsync(m => m.Id == id);

    public async Task<List<Mision>> ObtenerActivasPorHeroeIdAsync(Guid heroeId) =>
        await _context.Misiones
            .Where(m => m.HeroeId == heroeId && !m.Completada)
            .ToListAsync();

    public async Task AgregarAsync(Mision mision)
    {
        await _context.Misiones.AddAsync(mision);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Mision mision)
    {
        _context.Misiones.Update(mision);
        await _context.SaveChangesAsync();
    }
}