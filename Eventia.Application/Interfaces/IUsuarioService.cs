using Eventia.Application.DTOs.Usuario;
using Eventia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventia.Application.Interfaces
{
    public  interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DisableAsync(Guid id);
        Task<UsuarioDto?> BuscarPorCorreoAsync(string correo);

        Task<bool> AsignarRolAsync(Guid usuarioId, Rol nuevoRol);

    }
}
