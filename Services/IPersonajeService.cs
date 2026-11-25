using DSW_3P_PR02.DTOs;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Services;

public interface IPersonajeService
{
    Task<IEnumerable<Personaje>> ObtenerTodosAsync();
    Task<Personaje?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<Personaje>> ObtenerPorVideojuegoAsync(string videojuego);
    Task<IEnumerable<Personaje>> ObtenerPorNivelMinimoAsync(int nivel);
    Task<Personaje> RegistrarAsync(PersonajeCreateDto dto);
    Task<Personaje?> ActualizarAsync(int id, PersonajeUpdateDto dto);
    Task<bool> EliminarAsync(int id);
}
