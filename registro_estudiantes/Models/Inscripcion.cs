namespace registro_estudiantes.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        public int EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }
        public int MateriaId { get; set; }
        public Materia? Materia { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
    }
}
