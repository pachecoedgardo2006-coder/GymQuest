using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class RutinaEjercicioConfiguration : IEntityTypeConfiguration<RutinaEjercicio>
{
    public void Configure(EntityTypeBuilder<RutinaEjercicio> builder)
    {
        builder.ToTable("RutinaEjercicios");

        builder.HasKey(re => re.Id);

        builder.Property(re => re.SeriesObjetivo).IsRequired();
        builder.Property(re => re.RepeticionesObjetivo).IsRequired();

        builder.Property(re => re.PesoInicial)
            .IsRequired()
            .HasPrecision(6, 2);

        // EjercicioId es una referencia cruzada a otro agregado (Ejercicio):
        // solo columna Guid + índice, sin relación de EF.
        builder.HasIndex(re => re.EjercicioId);
    }
}