using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DSW_3P_PR02.Auth;
using DSW_3P_PR02.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DSW_3P_PR02.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwt;
    private readonly DemoUserSettings _demoUser;

    public AuthService(IOptions<JwtSettings> jwt, IOptions<DemoUserSettings> demoUser)
    {
        _jwt = jwt.Value;
        _demoUser = demoUser.Value;
    }

    public LoginResponseDto? Login(LoginDto dto)
    {
        if (!CredencialesValidas(dto.NombreUsuario, dto.Clave))
            return null;

        var expiracion = DateTime.UtcNow.AddMinutes(_jwt.ExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _demoUser.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, _demoUser.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiracion,
            signingCredentials: credenciales);

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiracion,
            ExpiresIn = _jwt.ExpirationMinutes * 60,
            Username = _demoUser.Username
        };
    }

    private bool CredencialesValidas(string usuario, string clave)
    {
        if (string.IsNullOrEmpty(_demoUser.Password)) return false;

        var usuarioOk = string.Equals(usuario, _demoUser.Username, StringComparison.OrdinalIgnoreCase);
        // Comparación en tiempo constante para no filtrar información por tiempos de respuesta.
        var claveOk = CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(clave), Encoding.UTF8.GetBytes(_demoUser.Password));
        return usuarioOk && claveOk;
    }
}
