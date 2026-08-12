using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class LogroConfiguration : IEntityTypeConfiguration<Logro>
{
    public void Configure(EntityTypeBuilder<Logro> builder)
    {
        builder.ToTable("Logros");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Descripcion)
            .HasMaxLength(500);

        builder.Property(l => l.Desbloqueado).IsRequired();
        builder.Property(l => l.FechaDesbloqueo); // nullable

        builder.HasIndex(l => l.HeroeId);
    }
}