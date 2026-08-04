using GymQuest.Application.DTOs;

namespace GymQuest.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto> RegistrarUsuarioAsync(CrearUsuarioDto dto);
    Task<UsuarioDto> IniciarSesionAsync(LoginDto dto);
}