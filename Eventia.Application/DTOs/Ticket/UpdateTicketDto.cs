namespace Eventia.Application.DTOs.Ticket;

public class UpdateTicketDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Estado { get; set; }
    public Guid UsuarioAsignadoId { get; set; }
    public DateTime? FechaCierre { get; set; }

    public Guid UsuarioCreadorId { get; set; }

}
