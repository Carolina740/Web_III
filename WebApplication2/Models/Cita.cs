using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

/// <summary>
/// Representa una cita médica agendada en la veterinaria.
/// </summary>
public class Cita
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una mascota.")]
    [Display(Name = "Mascota")]
    public int MascotaId { get; set; }

    // Navegación resuelta en el servicio
    public Mascota? Mascota { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un veterinario.")]
    [Display(Name = "Veterinario")]
    public int VeterinarioId { get; set; }

    // Navegación resuelta en el servicio
    public Veterinario? Veterinario { get; set; }

    [Required(ErrorMessage = "La fecha y hora de atención son obligatorias.")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha y Hora de Atención")]
    public DateTime FechaHora { get; set; } = DateTime.Now.AddHours(1);

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [StringLength(200, ErrorMessage = "El motivo no puede superar 200 caracteres.")]
    [Display(Name = "Motivo de la Consulta")]
    public string Motivo { get; set; } = string.Empty;

    [Display(Name = "Estado de la Cita")]
    public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;

    [StringLength(500, ErrorMessage = "El diagnóstico no puede superar 500 caracteres.")]
    [Display(Name = "Diagnóstico")]
    public string? Diagnostico { get; set; }

    /// <summary>Fecha formateada legible.</summary>
    public string FechaHoraFormateada => FechaHora.ToString("dd/MM/yyyy HH:mm");
}
