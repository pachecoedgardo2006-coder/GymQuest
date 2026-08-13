using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LogroController : ControllerBase
{
    private readonly ILogroService _logroService;

    public LogroController(ILogroService logroService)
    {
        _logroService = logroService;
    }

    [HttpGet("heroe/{heroeId:guid}")]
    public async Task<ActionResult<List<LogroDto>>> ObtenerLogros(Guid heroeId)
    {
        var logros = await _logroService.ObtenerLogrosAsync(heroeId);
        return Ok(logros);
    }

    [HttpPost("{logroId:guid}/desbloquear")]
    public async Task<ActionResult<LogroDto>> DesbloquearLogro(Guid logroId)
    {
        var logro = await _logroService.DesbloquearLogroAsync(logroId);
        return Ok(logro);
    }
}