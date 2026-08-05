using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface IRachaService
{
    Task<RachaDto> ObtenerRachaAsync(Guid heroeId);
    Task<RachaDto> RegistrarEntrenamientoDeHoyAsync(Guid heroeId, DateTime fecha);
}