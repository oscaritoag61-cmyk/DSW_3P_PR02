using DSW_3P_PR02.DTOs;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Inicia sesión con el usuario de demostración y devuelve un JWT.</summary>
    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponseDto> Login([FromBody] LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NombreUsuario) || string.IsNullOrEmpty(dto.Clave))
            return BadRequest(new { mensaje = "Usuario y contraseña son obligatorios." });

        var resultado = _authService.Login(dto);
        if (resultado is null)
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

        return Ok(resultado);
    }
}
