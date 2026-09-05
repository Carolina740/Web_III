using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;
public class Mascota
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar 50 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un propietario.")]
    [Display(Name = "Propietario")]
    public int PropietarioId { get; set; }
    public Propietario? Propietario { get; set; }

    [Required(ErrorMessage = "La especie es obligatoria.")]
    [StringLength(50, ErrorMessage = "La especie no puede superar 50 caracteres.")]
    [Display(Name = "Especie")]
    public string Especie { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "La raza no puede superar 50 caracteres.")]
    [Display(Name = "Raza")]
    public string Raza { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Nacimiento")]
    public DateTime FechaNacimiento { get; set; } = DateTime.Today;

    [StringLength(30, ErrorMessage = "El color no puede superar 30 caracteres.")]
    [Display(Name = "Color")]
    public string Color { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public EstadoGeneral Estado { get; set; } = EstadoGeneral.Activo;
    public int Edad => (int)((DateTime.Today - FechaNacimiento).TotalDays / 365.25);
}
