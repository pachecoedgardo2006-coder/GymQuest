using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EntrenamientoController : ControllerBase
{
    private readonly IEntrenamientoService _entrenamientoService;

    public EntrenamientoController(IEntrenamientoService entrenamientoService)
    {
        _entrenamientoService = entrenamientoService;
    }

    [HttpPost("iniciar")]
    public async Task<ActionResult<Guid>> IniciarEntrenamiento(IniciarEntrenamientoDto dto)
    {
        var sesionId = await _entrenamientoService.IniciarEntrenamientoAsync(dto);
        return Ok(sesionId);
    }

    [HttpPost("serie")]
    public async Task<IActionResult> RegistrarSerie(RegistrarSerieDto dto)
    {
        await _entrenamientoService.RegistrarSerieAsync(dto);
        return NoContent();
    }

    [HttpPost("{sesionId:guid}/finalizar")]
    public async Task<ActionResult<ResumenEntrenamientoDto>> FinalizarEntrenamiento(Guid sesionId)
    {
        var resumen = await _entrenamientoService.FinalizarEntrenamientoAsync(sesionId);
        return Ok(resumen);
    }
}