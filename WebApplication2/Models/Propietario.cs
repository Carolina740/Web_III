using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;
public class Propietario
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

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Formato de teléfono no válido.")]
    [StringLength(15, ErrorMessage = "El teléfono no puede superar 15 caracteres.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
    [StringLength(100, ErrorMessage = "El correo no puede superar 100 caracteres.")]
    [Display(Name = "Correo Electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public EstadoGeneral Estado { get; set; } = EstadoGeneral.Activo;
    public string NombreCompleto => $"{Apellido}, {Nombre}";
}
