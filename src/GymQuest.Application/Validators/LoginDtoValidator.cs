using FluentValidation;
using GymQuest.Application.DTOs;

namespace GymQuest.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        // A propósito NO se valida longitud mínima de Password aquí.
        // Si lo hiciéramos, un atacante podría distinguir "email no existe"
        // de "password demasiado corta" antes de siquiera llegar a
        // UsuarioService.IniciarSesionAsync, filtrando información y
        // rompiendo la protección anti user-enumeration ya implementada ahí
        // (mismo mensaje de error genérico para email inexistente o
        // password incorrecta).
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.");
    }
}