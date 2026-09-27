using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(150)]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio")]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; } = string.Empty;

        [Range(0, 9999, ErrorMessage = "La cantidad debe ser un número válido")]
        [Display(Name = "Cantidad disponible")]
        public int CantidadDisponible { get; set; } = 1;

        [Display(Name = "Año de publicación")]
        public int? AnioPublicacion { get; set; }

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
