using DSW_3P_PR02.DTOs;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

/// <summary>
/// Lógica de negocio. Depende solo de IPersonajeRepository, no de cómo se guardan los datos.
/// </summary>
public class PersonajeService : IPersonajeService
{
    private readonly IPersonajeRepository _repository;

    public PersonajeService(IPersonajeRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Personaje>> ObtenerTodosAsync() => _repository.GetAllAsync();

    public Task<Personaje?> ObtenerPorIdAsync(int id) => _repository.GetByIdAsync(id);

    public Task<IEnumerable<Personaje>> ObtenerPorVideojuegoAsync(string videojuego) =>
        _repository.GetByVideojuegoAsync(videojuego.Trim());

    public Task<IEnumerable<Personaje>> ObtenerPorNivelMinimoAsync(int nivel) =>
        _repository.GetByNivelMinimoAsync(nivel);

    public Task<Personaje> RegistrarAsync(PersonajeCreateDto dto)
    {
        var personaje = new Personaje
        {
            Nombre = dto.Nombre.Trim(),
            Videojuego = dto.Videojuego.Trim(),
            Clase = dto.Clase.Trim(),
            Nivel = dto.Nivel,
            Poder = dto.Poder,
            ImagenUrl = dto.ImagenUrl.Trim(),
            Disponible = true // Regla de negocio: todo personaje nuevo inicia disponible.
        };
        return _repository.AddAsync(personaje);
    }

    public async Task<Personaje?> ActualizarAsync(int id, PersonajeUpdateDto dto)
    {
        var existente = await _repository.GetByIdAsync(id);
        if (existente is null) return null;

        existente.Nombre = dto.Nombre.Trim();
        existente.Videojuego = dto.Videojuego.Trim();
        existente.Clase = dto.Clase.Trim();
        existente.Nivel = dto.Nivel;
        existente.Poder = dto.Poder;
        existente.Disponible = dto.Disponible;
        existente.ImagenUrl = dto.ImagenUrl.Trim();

        return await _repository.UpdateAsync(existente) ? existente : null;
    }

    public Task<bool> EliminarAsync(int id) => _repository.DeleteAsync(id);
}
