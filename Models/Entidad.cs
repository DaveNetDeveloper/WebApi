namespace API.Models
{
    public class Entidad
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public DateTime fechaAlta { get; set; }
        public int popularidad { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public Guid idTipoEntidad { get; set; }
    }
}