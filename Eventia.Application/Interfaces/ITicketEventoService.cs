using Eventia.Domain.Entities;

namespace Eventia.Application.Interfaces;

public interface ITicketEventoService
{
    Task RegistrarEventoAsync(TicketEvento evento);
    Task<IEnumerable<TicketEvento>> GetByTicketIdAsync(Guid ticketId);
}
