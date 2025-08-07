using System;

namespace Eventia.Domain.Entities
{
    public class TicketEvento
    {
        public Guid Id { get; set; }

        public Guid? TicketId { get; set; } 
        public Ticket? Ticket { get; set; }


        public Guid UsuarioResponsableId { get; set; }
        public Usuario UsuarioResponsable { get; set; }

        public string TipoEvento { get; set; } = string.Empty; 
        public string Mensaje { get; set; } = string.Empty; 

        public DateTime Fecha { get; set; }
    }
}
