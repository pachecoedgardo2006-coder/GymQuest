namespace GymQuest.Application.DTOs;

public record UsuarioDto(
    Guid Id,
    string NombreUsuario,
    string Email,
    DateTime FechaRegistro
);