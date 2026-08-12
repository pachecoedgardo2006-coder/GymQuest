using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class SerieRealizadaConfiguration : IEntityTypeConfiguration<SerieRealizada>
{
    public void Configure(EntityTypeBuilder<SerieRealizada> builder)
    {
        builder.ToTable("SeriesRealizadas");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.PesoLevantado)
            .IsRequired()
            .HasPrecision(6, 2);

        builder.Property(s => s.RepeticionesRealizadas).IsRequired();
        builder.Property(s => s.EsRecordPersonal).IsRequired();

        builder.HasIndex(s => s.EjercicioId);
    }
}