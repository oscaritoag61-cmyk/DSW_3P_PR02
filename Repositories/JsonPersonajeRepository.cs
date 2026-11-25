using System.Text.Json;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

/// <summary>
/// Persistencia en un archivo JSON. Se registra como Singleton y usa un semáforo
/// para que las lecturas/escrituras concurrentes no corrompan el archivo.
/// </summary>
public class JsonPersonajeRepository : IPersonajeRepository
{
    private readonly string _rutaArchivo;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private static readonly JsonSerializerOptions Opciones = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public JsonPersonajeRepository(IConfiguration configuration, IWebHostEnvironment env)
    {
        var ruta = configuration["JsonStorage:Path"] ?? "Data/personajes.json";
        _rutaArchivo = Path.IsPathRooted(ruta) ? ruta : Path.Combine(env.ContentRootPath, ruta);

        var carpeta = Path.GetDirectoryName(_rutaArchivo);
        if (!string.IsNullOrEmpty(carpeta)) Directory.CreateDirectory(carpeta);
        if (!File.Exists(_rutaArchivo)) File.WriteAllText(_rutaArchivo, "[]");
    }

    public async Task<IEnumerable<Personaje>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try { return await LeerAsync(); }
        finally { _lock.Release(); }
    }

    public async Task<Personaje?> GetByIdAsync(int id)
    {
        await _lock.WaitAsync();
        try { return (await LeerAsync()).FirstOrDefault(p => p.Id == id); }
        finally { _lock.Release(); }
    }

    public async Task<IEnumerable<Personaje>> GetByVideojuegoAsync(string videojuego)
    {
        await _lock.WaitAsync();
        try
        {
            return (await LeerAsync())
                .Where(p => string.Equals(p.Videojuego, videojuego, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        finally { _lock.Release(); }
    }

    public async Task<IEnumerable<Personaje>> GetByNivelMinimoAsync(int nivel)
    {
        await _lock.WaitAsync();
        try { return (await LeerAsync()).Where(p => p.Nivel >= nivel).ToList(); }
        finally { _lock.Release(); }
    }

    public async Task<Personaje> AddAsync(Personaje personaje)
    {
        await _lock.WaitAsync();
        try
        {
            var lista = await LeerAsync();
            personaje.Id = lista.Count == 0 ? 1 : lista.Max(p => p.Id) + 1;
            lista.Add(personaje);
            await GuardarAsync(lista);
            return personaje;
        }
        finally { _lock.Release(); }
    }

    public async Task<bool> UpdateAsync(Personaje personaje)
    {
        await _lock.WaitAsync();
        try
        {
            var lista = await LeerAsync();
            var indice = lista.FindIndex(p => p.Id == personaje.Id);
            if (indice < 0) return false;
            lista[indice] = personaje;
            await GuardarAsync(lista);
            return true;
        }
        finally { _lock.Release(); }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _lock.WaitAsync();
        try
        {
            var lista = await LeerAsync();
            if (lista.RemoveAll(p => p.Id == id) == 0) return false;
            await GuardarAsync(lista);
            return true;
        }
        finally { _lock.Release(); }
    }

    private async Task<List<Personaje>> LeerAsync()
    {
        await using var stream = File.OpenRead(_rutaArchivo);
        if (stream.Length == 0) return new List<Personaje>();
        return await JsonSerializer.DeserializeAsync<List<Personaje>>(stream, Opciones) ?? new List<Personaje>();
    }

    private async Task GuardarAsync(List<Personaje> lista)
    {
        // Se escribe a un temporal y luego se reemplaza, para no dejar el archivo a medias.
        var temporal = _rutaArchivo + ".tmp";
        await using (var stream = File.Create(temporal))
        {
            await JsonSerializer.SerializeAsync(stream, lista, Opciones);
        }
        File.Move(temporal, _rutaArchivo, overwrite: true);
    }
}
