using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface IHeroeService
{
    Task<HeroeEstadisticasDto> ConsultarEstadisticasAsync(Guid usuarioId);
}