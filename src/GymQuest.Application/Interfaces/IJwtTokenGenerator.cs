using GymQuest.Domain.Entities;

namespace GymQuest.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerarToken(Usuario usuario);
}