using GymQuest.Application.DTOs;
using GymQuest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous] // Todo este controller es público: nadie tiene token todavía
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public AuthController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("registro")]
    public async Task<ActionResult<AuthResponseDto>> Registrar(CrearUsuarioDto dto)
    {
        var respuesta = await _usuarioService.RegistrarUsuarioAsync(dto);
        return CreatedAtAction(nameof(Registrar), new { id = respuesta.Usuario.Id }, respuesta);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var respuesta = await _usuarioService.IniciarSesionAsync(dto);
        return Ok(respuesta);
    }
}