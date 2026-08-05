namespace GymQuest.Application.DTOs;

public record LogroDto(
    Guid Id,
    Guid HeroeId,
    string Nombre,
    string Descripcion,
    bool Desbloqueado,
    DateTime? FechaDesbloqueo);