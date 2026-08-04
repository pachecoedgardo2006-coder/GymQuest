using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using GymQuest.Domain.Entities;
using GymQuest.Domain.Interfaces;

namespace GymQuest.Application.Services;

public class EntrenamientoService : IEntrenamientoService
{
    private readonly ISesionEntrenamientoRepository _sesionRepository;
    private readonly IEjercicioRepository _ejercicioRepository;
    private readonly IHeroeRepository _heroeRepository;

    public EntrenamientoService(
        ISesionEntrenamientoRepository sesionRepository,
        IEjercicioRepository ejercicioRepository,
        IHeroeRepository heroeRepository)
    {
        _sesionRepository = sesionRepository;
        _ejercicioRepository = ejercicioRepository;
        _heroeRepository = heroeRepository;
    }

    public async Task<Guid> IniciarEntrenamientoAsync(IniciarEntrenamientoDto dto)
    {
        var sesion = new SesionEntrenamiento(dto.HeroeId, dto.RutinaId);
        await _sesionRepository.AgregarAsync(sesion);
        return sesion.Id;
    }

    public async Task RegistrarSerieAsync(RegistrarSerieDto dto)
    {
        var sesion = await _sesionRepository.ObtenerPorIdAsync(dto.SesionId)
            ?? throw new InvalidOperationException("La sesión de entrenamiento no existe.");

        var serie = new SerieRealizada(
            dto.EjercicioId,
            dto.PesoLevantado,
            dto.RepeticionesRealizadas,
            dto.EsRecordPersonal);

        sesion.RegistrarSerie(serie);
    }

    public async Task<ResumenEntrenamientoDto> FinalizarEntrenamientoAsync(Guid sesionId)
    {
        var sesion = await _sesionRepository.ObtenerPorIdAsync(sesionId)
            ?? throw new InvalidOperationException("La sesión de entrenamiento no existe.");

        var ejercicioIds = sesion.Series
            .Select(s => s.EjercicioId)
            .Distinct()
            .ToList();

        var ejercicios = await _ejercicioRepository.ObtenerPorIdsAsync(ejercicioIds);

        var factoresPorEjercicio = ejercicios
            .ToDictionary(e => e.Id, e => e.FactorXp);

        var xpGanada = sesion.Finalizar(factoresPorEjercicio);

        var heroe = await _heroeRepository.ObtenerPorIdAsync(sesion.HeroeId)
            ?? throw new InvalidOperationException("El héroe no existe.");

        var nivelAnterior = heroe.Nivel;
        heroe.GanarExperiencia(xpGanada);
        await _heroeRepository.ActualizarAsync(heroe);

        return new ResumenEntrenamientoDto(
            sesion.Id,
            xpGanada,
            nivelAnterior,
            heroe.Nivel,
            SubioDeNivel: heroe.Nivel > nivelAnterior);
    }
}