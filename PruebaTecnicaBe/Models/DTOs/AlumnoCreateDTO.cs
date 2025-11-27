namespace PruebaTecnicaBe.DTOs
{
    public class AlumnoCreateDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public bool isComplete{ get; set; } = false;
    }
}