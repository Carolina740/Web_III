using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required]
        public int LibroId { get; set; }
        public Libro? Libro { get; set; }

        [Display(Name = "Fecha de préstamo")]
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de devolución")]
        public DateTime? FechaDevolucion { get; set; }

        [Display(Name = "Fecha límite")]
        public DateTime FechaLimite { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo"; // Activo, Devuelto, Vencido

        [StringLength(300)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}
