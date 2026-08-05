using FluentValidation;
using GymQuest.Application.DTOs;

namespace GymQuest.Application.Validators;

public class IniciarEntrenamientoDtoValidator : AbstractValidator<IniciarEntrenamientoDto>
{
    public IniciarEntrenamientoDtoValidator()
    {
        RuleFor(x => x.HeroeId).NotEmpty();
        RuleFor(x => x.RutinaId).NotEmpty();
    }
}