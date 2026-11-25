using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Repositories;

/// <summary>
/// Persistencia en SQL Server mediante Entity Framework Core.
/// Implementa la misma interfaz que JsonPersonajeRepository.
/// </summary>
public class SqlPersonajeRepository : IPersonajeRepository
{
    private readonly CharacterHubContext _context;

    public SqlPersonajeRepository(CharacterHubContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Personaje>> GetAllAsync() =>
        await _context.Personajes.AsNoTracking().OrderBy(p => p.Id).ToListAsync();

    public Task<Personaje?> GetByIdAsync(int id) =>
        _context.Personajes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    // La intercalación por defecto de SQL Server no distingue mayúsculas/minúsculas.
    public async Task<IEnumerable<Personaje>> GetByVideojuegoAsync(string videojuego) =>
        await _context.Personajes.AsNoTracking()
            .Where(p => p.Videojuego == videojuego)
            .OrderBy(p => p.Id)
            .ToListAsync();

    public async Task<IEnumerable<Personaje>> GetByNivelMinimoAsync(int nivel) =>
        await _context.Personajes.AsNoTracking()
            .Where(p => p.Nivel >= nivel)
            .OrderBy(p => p.Id)
            .ToListAsync();

    public async Task<Personaje> AddAsync(Personaje personaje)
    {
        personaje.Id = 0; // Lo genera la columna IDENTITY.
        _context.Personajes.Add(personaje);
        await _context.SaveChangesAsync();
        return personaje;
    }

    public async Task<bool> UpdateAsync(Personaje personaje)
    {
        if (!await _context.Personajes.AnyAsync(p => p.Id == personaje.Id))
            return false;

        _context.Personajes.Update(personaje);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var filas = await _context.Personajes.Where(p => p.Id == id).ExecuteDeleteAsync();
        return filas > 0;
    }
}
