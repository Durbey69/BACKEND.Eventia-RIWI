using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Eventia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Eventia.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly EventiaDbContext _context;
        private readonly ITicketEventoService _ticketEventoService;
        public TicketService(EventiaDbContext context, ITicketEventoService ticketEventoService)
        {
            _context = context;
            _ticketEventoService = ticketEventoService;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                .Include(t => t.UsuarioAsignado)
                .Include(t => t.UsuarioCreador)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(Guid id)
        {
            return await _context.Tickets
                .Include(t => t.UsuarioAsignado)
                .Include(t => t.UsuarioCreador)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

 
            var evento = new TicketEvento
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                UsuarioResponsableId = ticket.UsuarioCreadorId,
                TipoEvento = "Creación",
                Mensaje = $"Ticket creado con título: {ticket.Titulo}",
                Fecha = DateTime.UtcNow
            };

            await _ticketEventoService.RegistrarEventoAsync(evento);

            return ticket;
        }


        public async Task<bool> UpdateAsync(Ticket ticket)
        {
            var exists = await _context.Tickets.AnyAsync(t => t.Id == ticket.Id);
            if (!exists) return false;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            // Registrar evento de actualización
            var evento = new TicketEvento
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                UsuarioResponsableId = ticket.UsuarioCreadorId,
                TipoEvento = "Actualización",
                Mensaje = $"El ticket fue actualizado.",
                Fecha = DateTime.UtcNow
            };

            await _ticketEventoService.RegistrarEventoAsync(evento);

            return true;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            // Primero registrar el evento
            var evento = new TicketEvento
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                UsuarioResponsableId = ticket.UsuarioCreadorId,
                TipoEvento = "Eliminación",
                Mensaje = $"El ticket con título '{ticket.Titulo}' fue eliminado.",
                Fecha = DateTime.UtcNow
            };

            await _ticketEventoService.RegistrarEventoAsync(evento);

            // Ahora sí eliminar el ticket
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }



    }
}
