using DSW_3P_PR02.DTOs;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Authorize]
[Route("api/personajes")]
[Produces("application/json")]
public class PersonajesController : ControllerBase
{
    private readonly IPersonajeService _service;

    public PersonajesController(IPersonajeService service)
    {
        _service = service;
    }

    /// <summary>Consulta todos los personajes.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Personaje>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Personaje>>> GetAll()
    {
        return Ok(await _service.ObtenerTodosAsync());
    }

    /// <summary>Consulta un personaje por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Personaje), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Personaje>> GetById(int id)
    {
        var personaje = await _service.ObtenerPorIdAsync(id);
        if (personaje is null)
            return NotFound(new { mensaje = $"No existe un personaje con id {id}." });
        return Ok(personaje);
    }

    /// <summary>Consulta los personajes de un videojuego.</summary>
    [HttpGet("videojuego/{nombre}")]
    [ProducesResponseType(typeof(IEnumerable<Personaje>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Personaje>>> GetByVideojuego(string nombre)
    {
        return Ok(await _service.ObtenerPorVideojuegoAsync(nombre));
    }

    /// <summary>Consulta los personajes con nivel igual o superior al indicado.</summary>
    [HttpGet("nivel/{nivel:int}")]
    [ProducesResponseType(typeof(IEnumerable<Personaje>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Personaje>>> GetByNivel(int nivel)
    {
        if (nivel < 1 || nivel > 100)
            return BadRequest(new { mensaje = "El nivel debe estar entre 1 y 100." });
        return Ok(await _service.ObtenerPorNivelMinimoAsync(nivel));
    }

    /// <summary>Registra un personaje nuevo (siempre queda disponible).</summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Personaje), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Personaje>> Create([FromBody] PersonajeCreateDto dto)
    {
        var creado = await _service.RegistrarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    /// <summary>Actualiza un personaje existente.</summary>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Personaje), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Personaje>> Update(int id, [FromBody] PersonajeUpdateDto dto)
    {
        var actualizado = await _service.ActualizarAsync(id, dto);
        if (actualizado is null)
            return NotFound(new { mensaje = $"No existe un personaje con id {id}." });
        return Ok(actualizado);
    }

    /// <summary>Elimina un personaje.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.EliminarAsync(id))
            return NotFound(new { mensaje = $"No existe un personaje con id {id}." });
        return NoContent();
    }
}
