using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class HeroeConfiguration : IEntityTypeConfiguration<Heroe>
{
    public void Configure(EntityTypeBuilder<Heroe> builder)
    {
        builder.ToTable("Heroes");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Titulo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(h => h.Nivel)
            .IsRequired();

        builder.Property(h => h.ExperienciaActual)
            .IsRequired();

        builder.Property(h => h.ExperienciaParaSiguienteNivel)
            .IsRequired();

        builder.Property(h => h.Fuerza).IsRequired();
        builder.Property(h => h.Resistencia).IsRequired();
        builder.Property(h => h.Consistencia).IsRequired();

        // Un usuario tiene un único héroe: garantizamos la regla a nivel de BD.
        builder.HasIndex(h => h.UsuarioId)
            .IsUnique();
    }
}