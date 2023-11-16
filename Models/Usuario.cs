namespace API.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        public bool activo { get; set; }
        public string contraseña { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public bool suscrito { get; set; }
        public DateTime? ultimaconexion { get; set; }
        public DateTime fechaCreación { get; set; }
        public int puntos { get; set; }
    }
}