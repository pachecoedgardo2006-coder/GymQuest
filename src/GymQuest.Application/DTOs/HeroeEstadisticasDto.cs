namespace GymQuest.Application.DTOs;

public record HeroeEstadisticasDto(
    Guid HeroeId,
    Guid UsuarioId,
    string Titulo,
    int Nivel,
    int ExperienciaActual,
    int ExperienciaParaSiguienteNivel,
    int Fuerza,
    int Resistencia,
    int Consistencia);