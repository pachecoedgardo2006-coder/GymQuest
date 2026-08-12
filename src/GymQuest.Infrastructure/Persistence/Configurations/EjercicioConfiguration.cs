using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class EjercicioConfiguration : IEntityTypeConfiguration<Ejercicio>
{
    public void Configure(EntityTypeBuilder<Ejercicio> builder)
    {
        builder.ToTable("Ejercicios");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        // Guardamos el enum como texto ("Multiarticular", "AltoVolumen"...)
        // en vez de número: si mañana reordenas el enum en el código,
        // los datos ya guardados en la BD no se corrompen.
        builder.Property(e => e.Categoria)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(e => e.FactorXp)
            .IsRequired()
            .HasPrecision(5, 2);
    }
}