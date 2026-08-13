using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface IUsuarioService
{
    Task<AuthResponseDto> RegistrarUsuarioAsync(CrearUsuarioDto dto);
    Task<AuthResponseDto> IniciarSesionAsync(LoginDto dto);
}