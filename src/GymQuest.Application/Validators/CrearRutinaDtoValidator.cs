using FluentValidation;
using GymQuest.Application.DTOs;

namespace GymQuest.Application.Validators;

public class CrearRutinaDtoValidator : AbstractValidator<CrearRutinaDto>
{
    public CrearRutinaDtoValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty();

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la rutina es obligatorio.")
            .MaximumLength(100);

        RuleFor(x => x.Ejercicios)
            .NotEmpty().WithMessage("Una rutina debe tener al menos un ejercicio.");

        RuleForEach(x => x.Ejercicios)
            .SetValidator(new CrearRutinaEjercicioDtoValidator());
    }
}

public class CrearRutinaEjercicioDtoValidator : AbstractValidator<CrearRutinaEjercicioDto>
{
    public CrearRutinaEjercicioDtoValidator()
    {
        RuleFor(x => x.EjercicioId).NotEmpty();
        RuleFor(x => x.SeriesObjetivo).GreaterThan(0);
        RuleFor(x => x.RepeticionesObjetivo).GreaterThan(0);
        RuleFor(x => x.PesoInicial).GreaterThanOrEqualTo(0);
    }
}