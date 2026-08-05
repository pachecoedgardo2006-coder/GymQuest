using FluentValidation;
using GymQuest.Application.DTOs;

namespace GymQuest.Application.Validators;

public class RegistrarSerieDtoValidator : AbstractValidator<RegistrarSerieDto>
{
    public RegistrarSerieDtoValidator()
    {
        RuleFor(x => x.SesionId).NotEmpty();
        RuleFor(x => x.EjercicioId).NotEmpty();
        RuleFor(x => x.PesoLevantado).GreaterThan(0);
        RuleFor(x => x.RepeticionesRealizadas).GreaterThan(0);
    }
}