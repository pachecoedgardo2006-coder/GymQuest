using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class HeroeRepository : IHeroeRepository
{
    private readonly GymQuestDbContext _context;

    public HeroeRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<Heroe?> ObtenerPorIdAsync(Guid id) =>
        await _context.Heroes.FirstOrDefaultAsync(h => h.Id == id);

    public async Task<Heroe?> ObtenerPorUsuarioIdAsync(Guid usuarioId) =>
        await _context.Heroes.FirstOrDefaultAsync(h => h.UsuarioId == usuarioId);

    public async Task AgregarAsync(Heroe heroe)
    {
        await _context.Heroes.AddAsync(heroe);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Heroe heroe)
    {
        _context.Heroes.Update(heroe);
        await _context.SaveChangesAsync();
    }
}