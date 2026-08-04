using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface IEntrenamientoService
{
    Task<Guid> IniciarEntrenamientoAsync(IniciarEntrenamientoDto dto);
    Task RegistrarSerieAsync(RegistrarSerieDto dto);
    Task<ResumenEntrenamientoDto> FinalizarEntrenamientoAsync(Guid sesionId);
}