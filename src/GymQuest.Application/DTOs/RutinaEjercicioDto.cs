namespace GymQuest.Application.DTOs;

public record RutinaEjercicioDto(
    Guid Id,
    Guid EjercicioId,
    int SeriesObjetivo,
    int RepeticionesObjetivo,
    decimal PesoInicial
);