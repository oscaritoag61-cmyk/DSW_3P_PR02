using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Data;

public class CharacterHubContext : DbContext
{
    public CharacterHubContext(DbContextOptions<CharacterHubContext> options) : base(options) { }

    public DbSet<Personaje> Personajes => Set<Personaje>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Personaje>(e =>
        {
            e.ToTable("Personajes", t =>
            {
                t.HasCheckConstraint("CK_Personajes_Nivel", "[Nivel] BETWEEN 1 AND 100");
                t.HasCheckConstraint("CK_Personajes_Poder", "[Poder] > 0");
            });
            e.HasKey(p => p.Id);
            e.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            e.Property(p => p.Videojuego).IsRequired().HasMaxLength(100);
            e.Property(p => p.Clase).IsRequired().HasMaxLength(50);
            e.Property(p => p.ImagenUrl).IsRequired().HasMaxLength(500);
            e.HasIndex(p => p.Videojuego);

            // Datos iniciales (los mismos que tenía el archivo JSON).
            e.HasData(
                new Personaje { Id = 1, Nombre = "Kratos", Videojuego = "God of War", Clase = "Guerrero", Nivel = 90, Poder = 9500, Disponible = true, ImagenUrl = "https://placehold.co/400x400/png?text=Kratos" },
                new Personaje { Id = 2, Nombre = "Geralt de Rivia", Videojuego = "The Witcher 3", Clase = "Brujo", Nivel = 75, Poder = 8200, Disponible = true, ImagenUrl = "https://placehold.co/400x400/png?text=Geralt" },
                new Personaje { Id = 3, Nombre = "Jaina Proudmoore", Videojuego = "World of Warcraft", Clase = "Maga", Nivel = 60, Poder = 7000, Disponible = true, ImagenUrl = "https://placehold.co/400x400/png?text=Jaina" }
            );
        });
    }
}
