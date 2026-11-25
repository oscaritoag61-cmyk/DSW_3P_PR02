namespace DSW_3P_PR02.DTOs;

/// <summary>
/// Datos para actualizar un personaje. Incluye Disponible para poder cambiarlo.
/// </summary>
public class PersonajeUpdateDto : PersonajeCreateDto
{
    public bool Disponible { get; set; } = true;
}
