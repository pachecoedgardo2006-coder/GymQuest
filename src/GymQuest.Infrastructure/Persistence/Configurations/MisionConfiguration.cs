using GymQuest.Domain.Entities;
using GymQuest.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class MisionConfiguration : IEntityTypeConfiguration<Mision>
{
    public void Configure(EntityTypeBuilder<Mision> builder)
    {
        builder.ToTable("Misiones");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Descripcion)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(m => m.TipoObjetivo)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(m => m.ValorObjetivo).IsRequired();
        builder.Property(m => m.ProgresoActual).IsRequired();
        builder.Property(m => m.Completada).IsRequired();
        builder.Property(m => m.FechaExpiracion).IsRequired();

        builder.HasIndex(m => m.HeroeId);
    }
}