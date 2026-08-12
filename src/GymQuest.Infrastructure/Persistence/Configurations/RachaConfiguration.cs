using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class RachaConfiguration : IEntityTypeConfiguration<Racha>
{
    public void Configure(EntityTypeBuilder<Racha> builder)
    {
        builder.ToTable("Rachas");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.DiasConsecutivos).IsRequired();
        builder.Property(r => r.DiasProgramadosPorSemana).IsRequired();
        builder.Property(r => r.PocionesDeDescansoDisponibles).IsRequired();

        // Nullable porque puede no haber entrenado nunca aún.
        builder.Property(r => r.UltimaFechaEntrenada);

        // Un héroe tiene una única racha.
        builder.HasIndex(r => r.HeroeId)
            .IsUnique();
    }
}