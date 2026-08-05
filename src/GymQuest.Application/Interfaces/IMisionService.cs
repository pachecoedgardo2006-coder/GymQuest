using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface IMisionService
{
    Task<List<MisionDto>> ObtenerMisionesActivasAsync(Guid heroeId);
    Task<MisionDto> RegistrarProgresoAsync(Guid misionId, int cantidad);
}