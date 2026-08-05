using FluentValidation;
using GymQuest.Application.DTOs;

namespace GymQuest.Application.Validators;

public class CrearUsuarioDtoValidator : AbstractValidator<CrearUsuarioDto>
{
    public CrearUsuarioDtoValidator()
    {
        RuleFor(x => x.NombreUsuario)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .EmailAddress().WithMessage("El formato del email no es válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
    }
}