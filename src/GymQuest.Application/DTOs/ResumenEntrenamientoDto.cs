namespace GymQuest.Application.DTOs;

public record ResumenEntrenamientoDto(
    Guid SesionId,
    int XpGanada,
    int NivelAnterior,
    int NivelActual,
    bool SubioDeNivel
);