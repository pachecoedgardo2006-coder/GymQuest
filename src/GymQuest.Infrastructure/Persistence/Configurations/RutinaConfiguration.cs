using GymQuest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymQuest.Infrastructure.Persistence.Configurations;

public class RutinaConfiguration : IEntityTypeConfiguration<Rutina>
{
    public void Configure(EntityTypeBuilder<Rutina> builder)
    {
        builder.ToTable("Rutinas");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.EsPlantillaPredeterminada)
            .IsRequired();

        // Relación real: Rutina "es dueña" de su lista de RutinaEjercicio.
        builder.HasMany(r => r.Ejercicios)
            .WithOne()
            .HasForeignKey(re => re.RutinaId)
            .OnDelete(DeleteBehavior.Cascade); // si borras la rutina, se borran sus ejercicios

        // Ejercicios expone IReadOnlyCollection<T>, pero el campo real es
        // el List<T> privado _ejercicios. Le decimos a EF que lea/escriba
        // directamente ahí (no existe un setter público que pueda usar).
        builder.Navigation(r => r.Ejercicios)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}