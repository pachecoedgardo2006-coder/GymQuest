using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RutinaController : ControllerBase
{
    private readonly IRutinaService _rutinaService;

    public RutinaController(IRutinaService rutinaService)
    {
        _rutinaService = rutinaService;
    }

    [HttpPost]
    public async Task<ActionResult<RutinaDto>> CrearRutina(CrearRutinaDto dto)
    {
        var rutina = await _rutinaService.CrearRutinaAsync(dto);
        return CreatedAtAction(nameof(CrearRutina), new { id = rutina.Id }, rutina);
    }
}