using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MisionController : ControllerBase
{
    private readonly IMisionService _misionService;

    public MisionController(IMisionService misionService)
    {
        _misionService = misionService;
    }

    [HttpGet("heroe/{heroeId:guid}/activas")]
    public async Task<ActionResult<List<MisionDto>>> ObtenerMisionesActivas(Guid heroeId)
    {
        var misiones = await _misionService.ObtenerMisionesActivasAsync(heroeId);
        return Ok(misiones);
    }

    [HttpPost("{misionId:guid}/progreso")]
    public async Task<ActionResult<MisionDto>> RegistrarProgreso(Guid misionId, [FromBody] int cantidad)
    {
        var mision = await _misionService.RegistrarProgresoAsync(misionId, cantidad);
        return Ok(mision);
    }
}