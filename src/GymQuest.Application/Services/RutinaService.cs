using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class RutinaService : IRutinaService
{
    private readonly IRutinaRepository _rutinaRepository;

    public RutinaService(IRutinaRepository rutinaRepository)
    {
        _rutinaRepository = rutinaRepository;
    }

    public async Task<RutinaDto> CrearRutinaAsync(CrearRutinaDto dto)
    {
        if (dto.Ejercicios is null || dto.Ejercicios.Count == 0)
        {
            throw new ArgumentException("Una rutina debe tener al menos un ejercicio.", nameof(dto));
        }

        var rutina = new Rutina(dto.UsuarioId, dto.Nombre, dto.EsPlantillaPredeterminada);

        foreach (var ejercicioDto in dto.Ejercicios)
        {
            var rutinaEjercicio = new RutinaEjercicio(
                rutina.Id,
                ejercicioDto.EjercicioId,
                ejercicioDto.SeriesObjetivo,
                ejercicioDto.RepeticionesObjetivo,
                ejercicioDto.PesoInicial);

            rutina.AgregarEjercicio(rutinaEjercicio);
        }

        await _rutinaRepository.AgregarAsync(rutina);

        return MapearADto(rutina);
    }

    private static RutinaDto MapearADto(Rutina rutina)
    {
        var ejerciciosDto = rutina.Ejercicios
            .Select(e => new RutinaEjercicioDto(
                e.Id,
                e.EjercicioId,
                e.SeriesObjetivo,
                e.RepeticionesObjetivo,
                e.PesoInicial))
            .ToList();

        return new RutinaDto(
            rutina.Id,
            rutina.UsuarioId,
            rutina.Nombre,
            rutina.EsPlantillaPredeterminada,
            ejerciciosDto);
    }
}