using DSW_3P_PR02.DTOs;

namespace DSW_3P_PR02.Services;

public interface IAuthService
{
    /// <summary>Devuelve el token si las credenciales son válidas; null en caso contrario.</summary>
    LoginResponseDto? Login(LoginDto dto);
}
