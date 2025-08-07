namespace Eventia.Application.DTOs.Usuario;

public class AsignarRolDto
{
    public Guid UsuarioId { get; set; }
    public int Rol { get; set; } // 0 = Agente, 1 = Supervisor, 2 = Administrador
}
