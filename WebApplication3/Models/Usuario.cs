using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo incorrecto")]
        [StringLength(150)]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(200)]
        public string Contrasena { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = "Usuario"; 

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
