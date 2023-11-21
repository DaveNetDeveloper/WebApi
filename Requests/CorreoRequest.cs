namespace API.Requests
{
    public class CorreoRequest
    {
        public string? Destinatario { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; }
        public Services.TipoEnvioCorreos TipoEnvio { get; set; }
    }
}
