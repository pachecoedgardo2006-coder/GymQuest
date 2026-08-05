using GymQuest.Domain.Enums;

namespace GymQuest.Application.DTOs;

public record MisionDto(
    Guid Id,
    Guid HeroeId,
    string Descripcion,
    TipoObjetivoMision TipoObjetivo,
    int ValorObjetivo,
    int ProgresoActual,
    bool Completada,
    DateTime FechaExpiracion);