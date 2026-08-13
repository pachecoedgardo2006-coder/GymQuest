using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HeroeController : ControllerBase
{
    private readonly IHeroeService _heroeService;

    public HeroeController(IHeroeService heroeService)
    {
        _heroeService = heroeService;
    }

    [HttpGet("usuario/{usuarioId:guid}/estadisticas")]
    public async Task<ActionResult<HeroeEstadisticasDto>> ConsultarEstadisticas(Guid usuarioId)
    {
        var estadisticas = await _heroeService.ConsultarEstadisticasAsync(usuarioId);
        return Ok(estadisticas);
    }
}