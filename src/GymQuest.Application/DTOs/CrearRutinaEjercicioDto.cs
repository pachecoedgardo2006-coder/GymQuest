namespace GymQuest.Application.DTOs;

public record CrearRutinaEjercicioDto(
    Guid EjercicioId,
    int SeriesObjetivo,
    int RepeticionesObjetivo,
    decimal PesoInicial
);