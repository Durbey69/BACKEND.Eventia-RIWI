namespace Eventia.Application.DTOs.Ticket;

public class CreateTicketDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Estado { get; set; } = 0;
    public Guid UsuarioAsignadoId { get; set; }

    public Guid UsuarioCreadorId { get; set; }

}
