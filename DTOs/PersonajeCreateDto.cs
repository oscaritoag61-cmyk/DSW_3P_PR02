using System.ComponentModel.DataAnnotations;
using DSW_3P_PR02.Validation;

namespace DSW_3P_PR02.DTOs;

/// <summary>
/// Datos para registrar un personaje. Disponible no se recibe: siempre inicia en true.
/// </summary>
public class PersonajeCreateDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El videojuego es obligatorio.")]
    [StringLength(100)]
    public string Videojuego { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La clase es obligatoria.")]
    [StringLength(50)]
    public string Clase { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "El nivel debe estar entre 1 y 100.")]
    public int Nivel { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El poder debe ser mayor que cero.")]
    public int Poder { get; set; }

    [Required(ErrorMessage = "ImagenUrl es obligatoria.")]
    [StringLength(500)]
    [ImagenUrl]
    public string ImagenUrl { get; set; } = string.Empty;
}
