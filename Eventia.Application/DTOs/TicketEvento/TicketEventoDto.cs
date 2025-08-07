namespace Eventia.Application.DTOs.TicketEvento;

public class TicketEventoDto
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public string TipoEvento { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }

    public Guid UsuarioResponsableId { get; set; }
    public string? NombreUsuarioResponsable { get; set; }
}
