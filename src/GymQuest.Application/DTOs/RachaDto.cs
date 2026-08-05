namespace GymQuest.Application.DTOs;

public record RachaDto(
    Guid Id,
    Guid HeroeId,
    int DiasConsecutivos,
    int DiasProgramadosPorSemana,
    int PocionesDeDescansoDisponibles,
    DateTime? UltimaFechaEntrenada
    );