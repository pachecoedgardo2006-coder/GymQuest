namespace GymQuest.Application.DTOs;

public record CrearRutinaDto(
    Guid UsuarioId,
    string Nombre,
    List<CrearRutinaEjercicioDto> Ejercicios,
    bool EsPlantillaPredeterminada = false
);