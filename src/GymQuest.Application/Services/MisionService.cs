using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _misionRepository;

    public MisionService(IMisionRepository misionRepository)
    {
        _misionRepository = misionRepository;
    }

    public async Task<List<MisionDto>> ObtenerMisionesActivasAsync(Guid heroeId)
    {
        var misiones = await _misionRepository.ObtenerActivasPorHeroeIdAsync(heroeId);
        return misiones.Select(MapearADto).ToList();
    }

    public async Task<MisionDto> RegistrarProgresoAsync(Guid misionId, int cantidad)
    {
        var mision = await _misionRepository.ObtenerPorIdAsync(misionId)
                     ?? throw new InvalidOperationException("La misión no existe.");

        mision.RegistrarProgreso(cantidad);
        await _misionRepository.ActualizarAsync(mision);

        return MapearADto(mision);
    }

    private static MisionDto MapearADto(Mision mision)
    {
        return new MisionDto(
            mision.Id,
            mision.HeroeId,
            mision.Descripcion,
            mision.TipoObjetivo,
            mision.ValorObjetivo,
            mision.ProgresoActual,
            mision.Completada,
            mision.FechaExpiracion);
    }
}