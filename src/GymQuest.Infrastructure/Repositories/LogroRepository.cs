using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class LogroRepository : ILogroRepository
{
    private readonly GymQuestDbContext _context;

    public LogroRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<Logro?> ObtenerPorIdAsync(Guid id) =>
        await _context.Logros.FirstOrDefaultAsync(l => l.Id == id);

    public async Task<List<Logro>> ObtenerPorHeroeIdAsync(Guid heroeId) =>
        await _context.Logros.Where(l => l.HeroeId == heroeId).ToListAsync();

    public async Task AgregarAsync(Logro logro)
    {
        await _context.Logros.AddAsync(logro);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Logro logro)
    {
        _context.Logros.Update(logro);
        await _context.SaveChangesAsync();
    }
}