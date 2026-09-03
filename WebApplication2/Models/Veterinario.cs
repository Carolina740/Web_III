using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

/// <summary>
/// Representa un veterinario que trabaja en la clínica.
/// </summary>
public class Veterinario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar 50 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(50, ErrorMessage = "El apellido no puede superar 50 caracteres.")]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    [StringLength(80, ErrorMessage = "La especialidad no puede superar 80 caracteres.")]
    [Display(Name = "Especialidad")]
    public string Especialidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Formato de teléfono no válido.")]
    [StringLength(15, ErrorMessage = "El teléfono no puede superar 15 caracteres.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public EstadoGeneral Estado { get; set; } = EstadoGeneral.Activo;

    /// <summary>Nombre completo calculado: Dr. Apellido, Nombre.</summary>
    public string NombreCompleto => $"Dr. {Apellido}, {Nombre}";
}
