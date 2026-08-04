namespace GymQuest.Application.DTOs;

public record RutinaDto(
    Guid Id,
    Guid UsuarioId,
    string Nombre,
    bool EsPlantillaPredeterminada,
    List<RutinaEjercicioDto> Ejercicios
);