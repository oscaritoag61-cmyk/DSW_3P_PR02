using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

/// <summary>
/// Contrato de acceso a datos. No cambia entre la versión JSON y la de SQL Server.
/// </summary>
public interface IPersonajeRepository
{
    Task<IEnumerable<Personaje>> GetAllAsync();
    Task<Personaje?> GetByIdAsync(int id);
    Task<IEnumerable<Personaje>> GetByVideojuegoAsync(string videojuego);
    Task<IEnumerable<Personaje>> GetByNivelMinimoAsync(int nivel);
    Task<Personaje> AddAsync(Personaje personaje);
    Task<bool> UpdateAsync(Personaje personaje);
    Task<bool> DeleteAsync(int id);
}
