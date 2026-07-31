namespace GymQuest.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string NombreUsuario { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime FechaRegistro { get; private set; }

    protected Usuario() { }

    public Usuario(string nombreUsuario, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        NombreUsuario = nombreUsuario;
        Email = email;
        PasswordHash = passwordHash;
        FechaRegistro = DateTime.UtcNow;
    }
}