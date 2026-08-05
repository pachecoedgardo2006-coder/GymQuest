using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class RachaService : IRachaService
{
    private const int DiasProgramadosPorDefecto = 3;

    private readonly IRachaRepository _rachaRepository;

    public RachaService(IRachaRepository rachaRepository)
    {
        _rachaRepository = rachaRepository;
    }

    public async Task<RachaDto> ObtenerRachaAsync(Guid heroeId)
    {
        var racha = await _rachaRepository.ObtenerPorHeroeIdAsync(heroeId)
                    ?? throw new InvalidOperationException("El héroe no tiene una racha registrada todavía.");

        return MapearADto(racha);
    }

    public async Task<RachaDto> RegistrarEntrenamientoDeHoyAsync(Guid heroeId, DateTime fecha)
    {
        var racha = await _rachaRepository.ObtenerPorHeroeIdAsync(heroeId);

        if (racha is null)
        {
            // Primera vez que este héroe entrena: se crea su racha con un
            // valor por defecto de días programados (el usuario podrá
            // ajustarlo después desde configuración, si se agrega esa opción).
            racha = new Racha(heroeId, DiasProgramadosPorDefecto);
            racha.RegistrarEntrenamientoHoy(fecha);
            await _rachaRepository.AgregarAsync(racha);
        }
        else
        {
            racha.RegistrarEntrenamientoHoy(fecha);
            await _rachaRepository.ActualizarAsync(racha);
        }

        return MapearADto(racha);
    }

    private static RachaDto MapearADto(Racha racha)
    {
        return new RachaDto(
            racha.Id,
            racha.HeroeId,
            racha.DiasConsecutivos,
            racha.DiasProgramadosPorSemana,
            racha.PocionesDeDescansoDisponibles,
            racha.UltimaFechaEntrenada);
    }
}