using Eventia.Application.DTOs.Usuario;
using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Eventia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventia.Infrastructure.Services
{
    public  class UsuarioService : IUsuarioService
    {
        private readonly EventiaDbContext _context;

    public UsuarioService(EventiaDbContext context)
    {
        _context = context;
    }

        public async Task<UsuarioDto?> BuscarPorCorreoAsync(string correo)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo);

            if (usuario is null) return null;

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = (int)usuario.Rol
                
            };
        }


        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }


        public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> UpdateAsync(Usuario usuario)
    {
        var exists = await _context.Usuarios.AnyAsync(u => u.Id == usuario.Id);
        if (!exists) return false;

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DisableAsync(Guid id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        usuario.Activo = false;
        await _context.SaveChangesAsync();
        return true;
    }

        public async Task<bool> AsignarRolAsync(Guid usuarioId, Rol nuevoRol)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null) return false;

            usuario.Rol = nuevoRol;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
