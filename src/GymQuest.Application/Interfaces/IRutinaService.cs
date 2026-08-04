using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface IRutinaService
{
    Task<RutinaDto> CrearRutinaAsync(CrearRutinaDto dto);
}