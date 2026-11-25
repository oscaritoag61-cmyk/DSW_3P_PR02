namespace DSW_3P_PR02.DTOs;

/// <summary>
/// Credenciales de inicio de sesión. Acepta "username" o "usuario" para el nombre,
/// y "password" o "contrasena" para la contraseña, por compatibilidad con distintos clientes.
/// </summary>
public class LoginDto
{
    public string? Username { get; set; }
    public string? Usuario { get; set; }
    public string? Password { get; set; }
    public string? Contrasena { get; set; }

    public string NombreUsuario => (Username ?? Usuario ?? string.Empty).Trim();
    public string Clave => Password ?? Contrasena ?? string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public DateTime Expiration { get; set; }
    public int ExpiresIn { get; set; }
    public string Username { get; set; } = string.Empty;
}
