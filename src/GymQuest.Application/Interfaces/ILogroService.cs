using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface ILogroService
{
    Task<List<LogroDto>> ObtenerLogrosAsync(Guid heroeId);
    Task<LogroDto> DesbloquearLogroAsync(Guid logroId);
}