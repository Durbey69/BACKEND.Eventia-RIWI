using AutoMapper;
using Eventia.Application.DTOs.Ticket;
using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ITicketEventoService _ticketEventoService;
    private readonly IMapper _mapper;

    public TicketsController(
        ITicketService ticketService,
        ITicketEventoService ticketEventoService,
        IMapper mapper)
    {
        _ticketService = ticketService;
        _ticketEventoService = ticketEventoService;
        _mapper = mapper;
    }

    // GET: api/tickets
    //[Authorize(Roles = "Agente,Supervisor,Administrador")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tickets = await _ticketService.GetAllAsync();

        var result = tickets.Select(t => new TicketDto
        {
            Id = t.Id,
            Titulo = t.Titulo,
            Descripcion = t.Descripcion,
            Estado = (int)t.Estado,
            UsuarioAsignadoId = t.UsuarioAsignadoId,
            UsuarioCreadorId = t.UsuarioCreadorId,
            FechaCreacion = t.FechaCreacion,
            FechaCierre = t.FechaCierre,
            NombreUsuarioAsignado = t.UsuarioAsignado?.Nombre,
            NombreUsuarioCreador = t.UsuarioCreador?.Nombre
        });

        return Ok(result);
    }

    // GET: api/tickets/{id}
    //[Authorize(Roles = "Agente,Supervisor,Administrador")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { mensaje = "Ticket no encontrado" });

        var dto = new TicketDto
        {
            Id = ticket.Id,
            Titulo = ticket.Titulo,
            Descripcion = ticket.Descripcion,
            Estado = (int)ticket.Estado,
            UsuarioAsignadoId = ticket.UsuarioAsignadoId,
            UsuarioCreadorId = ticket.UsuarioCreadorId,
            FechaCreacion = ticket.FechaCreacion,
            FechaCierre = ticket.FechaCierre,
            NombreUsuarioAsignado = ticket.UsuarioAsignado?.Nombre,
            NombreUsuarioCreador = ticket.UsuarioCreador?.Nombre
        };

        //  Trazabilidad: registrar visualización
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var responsableId = Guid.TryParse(userId, out var parsed)
            ? parsed
            : Guid.Parse("90917090-37a2-470d-b36d-4a74915fcb48"); // ID simulado

        await _ticketEventoService.RegistrarEventoAsync(new TicketEvento
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            UsuarioResponsableId = responsableId,
            TipoEvento = "Visualización",
            Mensaje = $"Ticket visualizado por {User.Identity?.Name ?? "Admin simulado"}",
            Fecha = DateTime.UtcNow
        });

        return Ok(dto);
    }


    // POST: api/tickets
    //[Authorize(Roles = "Agente,Administrador")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
    {
        try
        {
            var ticket = _mapper.Map<Ticket>(dto);
            ticket.FechaCreacion = DateTime.UtcNow;

            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId == null) return Unauthorized();

            //ticket.UsuarioCreadorId = Guid.Parse(userId);
            ticket.UsuarioCreadorId = dto.UsuarioCreadorId;


            var creado = await _ticketService.CreateAsync(ticket);
            var result = _mapper.Map<TicketDto>(creado);

            await _ticketEventoService.RegistrarEventoAsync(new TicketEvento
            {
                Id = Guid.NewGuid(),
                TicketId = creado.Id,
                UsuarioResponsableId = ticket.UsuarioCreadorId,
                TipoEvento = "Creación",
                Mensaje = $"Ticket creado por {User.Identity?.Name}",
                Fecha = DateTime.UtcNow
            });

            return CreatedAtAction(nameof(Get), new { id = result.Id }, new
            {
                mensaje = "Ticket creado exitosamente",
                ticket = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al crear el ticket", detalle = ex.Message });
        }
    }

    //[Authorize(Roles = "Administrador")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketDto dto)
    {
        if (id != dto.Id)
            return BadRequest(new { mensaje = "El ID del ticket no coincide" });

        try
        {
            var ticket = _mapper.Map<Ticket>(dto);
            var actualizado = await _ticketService.UpdateAsync(ticket);

            if (!actualizado)
                return NotFound(new { mensaje = "No se encontró el ticket para actualizar" });

            // Fallback para pruebas sin login
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var responsableId = Guid.TryParse(userId, out var parsed)
                ? parsed
                : Guid.Parse("90917090-37a2-470d-b36d-4a74915fcb48"); 

            await _ticketEventoService.RegistrarEventoAsync(new TicketEvento
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                UsuarioResponsableId = responsableId,
                TipoEvento = "Actualización",
                Mensaje = $"Ticket actualizado por {User.Identity?.Name ?? "Admin simulado"}",
                Fecha = DateTime.UtcNow
            });

            return Ok(new { mensaje = "Ticket actualizado correctamente" });
        }
        catch (Exception ex)
        {
            var inner = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"ERROR: {inner}");
            return StatusCode(500, new { mensaje = "Error al actualizar el ticket", detalle = inner });
        }
    }


    //[Authorize(Roles = "Administrador")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid usuarioResponsableId)
    {
        try
        {
            // Registrar evento de eliminación ANTES de eliminar el ticket
            await _ticketEventoService.RegistrarEventoAsync(new TicketEvento
            {
                Id = Guid.NewGuid(),
                TicketId = id,
                UsuarioResponsableId = usuarioResponsableId,
                TipoEvento = "Eliminación",
                Mensaje = $"Ticket eliminado por usuario ID: {usuarioResponsableId}",
                Fecha = DateTime.UtcNow
            });

            var eliminado = await _ticketService.DeleteAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "No se encontró el ticket para eliminar" });

            return Ok(new { mensaje = "Ticket eliminado correctamente" });
        }
        catch (Exception ex)
        {
            var errorMsg = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"Error interno: {errorMsg}");
            return StatusCode(500, new { mensaje = "Error al eliminar el ticket", detalle = errorMsg });
        }
    }



}
