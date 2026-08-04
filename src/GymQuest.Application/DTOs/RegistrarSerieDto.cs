namespace GymQuest.Application.DTOs;

public record RegistrarSerieDto(
    Guid SesionId,
    Guid EjercicioId,
    decimal PesoLevantado,
    int RepeticionesRealizadas,
    bool EsRecordPersonal = false
);