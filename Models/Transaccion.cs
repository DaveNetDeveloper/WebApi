namespace API.Models
{
    public class Transaccion
    {
        public int? id { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public int? idProducto { get; set; }
        public DateTime? fecha { get; set; }
        public int puntos { get; set; }
    }
}