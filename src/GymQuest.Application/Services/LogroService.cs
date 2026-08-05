using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class LogroService : ILogroService
{
    private readonly ILogroRepository _logroRepository;

    public LogroService(ILogroRepository logroRepository)
    {
        _logroRepository = logroRepository;
    }

    public async Task<List<LogroDto>> ObtenerLogrosAsync(Guid heroeId)
    {
        var logros = await _logroRepository.ObtenerPorHeroeIdAsync(heroeId);
        return logros.Select(MapearADto).ToList();
    }

    public async Task<LogroDto> DesbloquearLogroAsync(Guid logroId)
    {
        var logro = await _logroRepository.ObtenerPorIdAsync(logroId)
                    ?? throw new InvalidOperationException("El logro no existe.");

        logro.Desbloquear();
        await _logroRepository.ActualizarAsync(logro);

        return MapearADto(logro);
    }

    private static LogroDto MapearADto(Logro logro)
    {
        return new LogroDto(
            logro.Id,
            logro.HeroeId,
            logro.Nombre,
            logro.Descripcion,
            logro.Desbloqueado,
            logro.FechaDesbloqueo);
    }
}