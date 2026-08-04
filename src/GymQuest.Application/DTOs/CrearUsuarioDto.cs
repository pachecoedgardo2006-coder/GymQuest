namespace GymQuest.Application.DTOs;

public record CrearUsuarioDto(
    string NombreUsuario,
    string Email,
    string Password
);