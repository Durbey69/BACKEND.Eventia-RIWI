using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Eventia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Eventia.Infrastructure.Services
{
    public class TicketEventoService : ITicketEventoService
    {
        private readonly EventiaDbContext _context;

        public TicketEventoService(EventiaDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarEventoAsync(TicketEvento evento)
        {
            _context.TicketEventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketEvento>> GetByTicketIdAsync(Guid ticketId)
        {
            return await _context.TicketEventos
                .Include(te => te.UsuarioResponsable)
                .Where(te => te.TicketId == ticketId)
                .OrderByDescending(te => te.Fecha)
                .ToListAsync();
        }
    }
}
