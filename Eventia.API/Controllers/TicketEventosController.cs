using AutoMapper;
using Eventia.Application.DTOs.TicketEvento;
using Eventia.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Supervisor")]
public class TicketEventosController : ControllerBase
{
    private readonly ITicketEventoService _eventoService;
    private readonly IMapper _mapper;

    public TicketEventosController(ITicketEventoService eventoService, IMapper mapper)
    {
        _eventoService = eventoService;
        _mapper = mapper;
    }

    [HttpGet("ticket/{ticketId}")]
    public async Task<IActionResult> GetByTicketId(Guid ticketId)
    {
        var eventos = await _eventoService.GetByTicketIdAsync(ticketId);

        var result = eventos.Select(e => new TicketEventoDto
        {
            Id = e.Id,
            TicketId = (Guid)e.TicketId!,
            TipoEvento = e.TipoEvento,
            Mensaje = e.Mensaje,
            Fecha = e.Fecha,
            UsuarioResponsableId = e.UsuarioResponsableId,
            NombreUsuarioResponsable = e.UsuarioResponsable?.Nombre
        });

        return Ok(result);
    }
}
