using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventia.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public EstadoTicket Estado { get; set; } = new EstadoTicket();

        public Guid UsuarioAsignadoId { get; set; }
        public Usuario? UsuarioAsignado { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaCierre { get; set; }

        public Guid UsuarioCreadorId { get; set; }
        public Usuario? UsuarioCreador { get; set; }

        public ICollection<TicketEvento> Eventos { get; set; } = new List<TicketEvento>();

    }
}
