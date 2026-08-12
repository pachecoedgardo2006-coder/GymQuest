using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;
using GymQuest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly GymQuestDbContext _context;

    public UsuarioRepository(GymQuestDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorIdAsync(Guid id) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<Usuario?> ObtenerPorEmailAsync(string email) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }
}