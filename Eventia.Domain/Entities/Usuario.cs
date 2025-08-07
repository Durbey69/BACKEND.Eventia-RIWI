using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventia.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public Rol Rol { get; set; } = Rol.Agente; // Rol del usuario en el sistema

        public bool Activo { get; set; } = true; // Indica si el usuario está activo

        public ICollection<Ticket> TicketsAsignados { get; set; }
    }
}
