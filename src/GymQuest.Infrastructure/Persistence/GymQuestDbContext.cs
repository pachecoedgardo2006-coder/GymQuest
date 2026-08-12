using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Infrastructure.Persistence;

public class GymQuestDbContext : DbContext
{
    public GymQuestDbContext(DbContextOptions<GymQuestDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Heroe> Heroes => Set<Heroe>();
    public DbSet<Ejercicio> Ejercicios => Set<Ejercicio>();
    public DbSet<Rutina> Rutinas => Set<Rutina>();
    public DbSet<SesionEntrenamiento> SesionesEntrenamiento => Set<SesionEntrenamiento>();
    public DbSet<Racha> Rachas => Set<Racha>();
    public DbSet<Logro> Logros => Set<Logro>();
    public DbSet<Mision> Misiones => Set<Mision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymQuestDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}