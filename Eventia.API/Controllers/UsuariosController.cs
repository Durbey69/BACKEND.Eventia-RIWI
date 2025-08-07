using AutoMapper;
using Eventia.Application.DTOs.Usuario;
using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IMapper _mapper;

    public UsuariosController(IUsuarioService usuarioService, IMapper mapper)
    {
        _usuarioService = usuarioService;
        _mapper = mapper;
    }

    [HttpGet("BuscarPorCorreo")]
    public async Task<ActionResult<UsuarioDto>> BuscarPorCorreo([FromQuery] string correo)
    {
        var usuario = await _usuarioService.BuscarPorCorreoAsync(correo);

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usuarios = await _usuarioService.GetAllAsync();
        var result = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado" });

        var dto = _mapper.Map<UsuarioDto>(usuario);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
    {
        var usuario = _mapper.Map<Usuario>(dto);
        var creado = await _usuarioService.CreateAsync(usuario);
        var result = _mapper.Map<UsuarioDto>(creado);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUsuarioDto dto)
    {
        if (id != dto.Id)
            return BadRequest(new { mensaje = "El ID no coincide con el del usuario" });

        var usuario = _mapper.Map<Usuario>(dto);
        var actualizado = await _usuarioService.UpdateAsync(usuario);
        if (!actualizado)
            return NotFound(new { mensaje = "Usuario no encontrado" });

        return Ok(new { mensaje = "Usuario actualizado correctamente" });
    }

    [HttpPatch("{id}/disable")]
    public async Task<IActionResult> Disable(Guid id)
    {
        var result = await _usuarioService.DisableAsync(id);
        if (!result)
            return NotFound(new { mensaje = "Usuario no encontrado o ya está deshabilitado" });

        return Ok(new { mensaje = "Usuario deshabilitado correctamente" });
    }

    [HttpPatch("{id}/enable")]
    public async Task<IActionResult> Enable(Guid id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado" });

        usuario.Activo = true;
        await _usuarioService.UpdateAsync(usuario); // reutilizar el método Update para guardar cambios
        return Ok(new { mensaje = "Usuario habilitado correctamente" });
    }

    [HttpPatch("{id}/rol")]
    //[Authorize(Roles = "Administrador")] 
    public async Task<IActionResult> AsignarRol(Guid id, [FromBody] AsignarRolDto dto)
    {
        if (id != dto.UsuarioId)
            return BadRequest(new { mensaje = "El ID de la URL no coincide con el del cuerpo" });

        var actualizado = await _usuarioService.AsignarRolAsync(dto.UsuarioId, (Rol)dto.Rol);
        if (!actualizado)
            return NotFound(new { mensaje = "Usuario no encontrado" });

        return Ok(new { mensaje = "Rol asignado correctamente" });
    }
}
