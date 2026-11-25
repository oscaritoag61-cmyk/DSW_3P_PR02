namespace DSW_3P_PR02.Auth;

/// <summary>
/// Configuración del token. Issuer, Audience y ExpirationMinutes viven en appsettings.json;
/// Key (la clave de firma) NO está en el repositorio: se lee de user-secrets o de la
/// variable de entorno Jwt__Key.
/// </summary>
public class JwtSettings
{
    public const string Section = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}

/// <summary>
/// Usuario de demostración. La contraseña se lee de user-secrets o de DemoUser__Password.
/// </summary>
public class DemoUserSettings
{
    public const string Section = "DemoUser";

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
