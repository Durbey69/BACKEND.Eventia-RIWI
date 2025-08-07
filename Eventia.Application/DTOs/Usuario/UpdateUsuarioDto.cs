using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventia.Application.DTOs.Usuario
{
    public class UpdateUsuarioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int Rol { get; set; }
        public bool Activo { get; set; }
    }
}
