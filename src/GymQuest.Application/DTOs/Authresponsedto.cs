namespace GymQuest.Application.DTOs;

public record AuthResponseDto(
    UsuarioDto Usuario,
    string Token
);