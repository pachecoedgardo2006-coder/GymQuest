using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class SesionEntrenamientoConfiguration : IEntityTypeConfiguration<SesionEntrenamiento>
{
    public void Configure(EntityTypeBuilder<SesionEntrenamiento> builder)
    {
        builder.ToTable("SesionesEntrenamiento");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Fecha).IsRequired();
        builder.Property(s => s.Completada).IsRequired();

        builder.HasIndex(s => s.HeroeId);

        // Relación real: la sesión "es dueña" de sus series realizadas.
        // SerieRealizada no tiene una propiedad SesionEntrenamientoId visible
        // en el código (no la necesita para su lógica de dominio), así que
        // EF va a crear esa columna FK "en la sombra" (shadow property).
        builder.HasMany(s => s.Series)
            .WithOne()
            .HasForeignKey("SesionEntrenamientoId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Series)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}