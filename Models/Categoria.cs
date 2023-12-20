namespace API.Models
{
    public class Categoria
    {
        public Guid id { get; set; }
        public Guid idTipoEntidad { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
    }
}