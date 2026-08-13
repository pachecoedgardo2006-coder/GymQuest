using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RachaController : ControllerBase
{
    private readonly IRachaService _rachaService;

    public RachaController(IRachaService rachaService)
    {
        _rachaService = rachaService;
    }

    [HttpGet("heroe/{heroeId:guid}")]
    public async Task<ActionResult<RachaDto>> ObtenerRacha(Guid heroeId)
    {
        var racha = await _rachaService.ObtenerRachaAsync(heroeId);
        return Ok(racha);
    }

    // fecha es opcional: si no se manda, se usa la fecha de hoy (UTC) en el servidor.
    [HttpPost("heroe/{heroeId:guid}/entrenamiento-hoy")]
    public async Task<ActionResult<RachaDto>> RegistrarEntrenamientoDeHoy(Guid heroeId, [FromQuery] DateTime? fecha)
    {
        var fechaAUsar = fecha ?? DateTime.UtcNow.Date;
        var racha = await _rachaService.RegistrarEntrenamientoDeHoyAsync(heroeId, fechaAUsar);
        return Ok(racha);
    }
}