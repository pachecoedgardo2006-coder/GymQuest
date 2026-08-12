using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class RachaRepository : IRachaRepository
{
    private readonly GymQuestDbContext _context;

    public RachaRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<Racha?> ObtenerPorHeroeIdAsync(Guid heroeId) =>
        await _context.Rachas.FirstOrDefaultAsync(r => r.HeroeId == heroeId);

    public async Task AgregarAsync(Racha racha)
    {
        await _context.Rachas.AddAsync(racha);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Racha racha)
    {
        _context.Rachas.Update(racha);
        await _context.SaveChangesAsync();
    }
}