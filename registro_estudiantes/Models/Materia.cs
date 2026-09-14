namespace registro_estudiantes.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; } = 0;
        public string Sigla { get; set; } = string.Empty;
        public string Profesor { get; set; } = string.Empty;
    }
}
