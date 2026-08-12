using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class EjercicioRepository : IEjercicioRepository
{
    private readonly GymQuestDbContext _context;

    public EjercicioRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<Ejercicio?> ObtenerPorIdAsync(Guid id) =>
        await _context.Ejercicios.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Ejercicio>> ObtenerTodosAsync() =>
        await _context.Ejercicios.ToListAsync();

    public async Task<IEnumerable<Ejercicio>> ObtenerPorIdsAsync(IEnumerable<Guid> ids) =>
        await _context.Ejercicios.Where(e => ids.Contains(e.Id)).ToListAsync();

    public async Task AgregarAsync(Ejercicio ejercicio)
    {
        await _context.Ejercicios.AddAsync(ejercicio);
        await _context.SaveChangesAsync();
    }
}