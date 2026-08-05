using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class HeroeService : IHeroeService
{
    private readonly IHeroeRepository _heroeRepository;

    public HeroeService(IHeroeRepository heroeRepository)
    {
        _heroeRepository = heroeRepository;
    }

    public async Task<HeroeEstadisticasDto> ConsultarEstadisticasAsync(Guid usuarioId)
    {
        var heroe = await _heroeRepository.ObtenerPorUsuarioIdAsync(usuarioId);

        if (heroe is null)
        {
            throw new InvalidOperationException("El héroe solicitado no existe.");
        }

        return MapearADto(heroe);
    }

    private static HeroeEstadisticasDto MapearADto(Heroe heroe)
    {
        return new HeroeEstadisticasDto(
            heroe.Id,
            heroe.UsuarioId,
            heroe.Titulo,
            heroe.Nivel,
            heroe.ExperienciaActual,
            heroe.ExperienciaParaSiguienteNivel,
            heroe.Fuerza,
            heroe.Resistencia,
            heroe.Consistencia);
    }
}