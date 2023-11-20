using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    public class CorreoRequest {
        public string? Destinatario { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; }
    }

    public class CorreoService {
        public void EnviarCorreo(string destinatario, string asunto, string cuerpo, string servidorSmtp, int puertoSmtp, string usuarioSmtp, string contraseñaSmtp) 
        {
            using (var mensaje = new MailMessage()) {
                mensaje.From = new MailAddress(usuarioSmtp);
                mensaje.To.Add(destinatario);
                mensaje.Subject = asunto;
                mensaje.Body = cuerpo;
                mensaje.IsBodyHtml = true;

                using (var clienteSmtp = new SmtpClient(servidorSmtp, puertoSmtp)) {
                    clienteSmtp.Credentials = new NetworkCredential(usuarioSmtp, contraseñaSmtp);
                    clienteSmtp.EnableSsl = true;
                    //clienteSmtp.Send(mensaje);
                }
            }
        }

        public void EnviarCorreoConPlantilla(string destinatario, string nombreUsuario, string token)
        {
            string asunto = "Bienvenido a Nuestra Aplicación";
            string cuerpo = ConstruirCuerpoHTML(nombreUsuario, token);

            using (var mensaje = new MailMessage()) {
                mensaje.From = new MailAddress("dgomezmart@uoc.edu");  
                mensaje.To.Add(destinatario);
                mensaje.Subject = asunto;
                mensaje.Body = cuerpo;
                mensaje.IsBodyHtml = true;

                using (var clienteSmtp = new SmtpClient("smtp.gmail.com", 587)) {
                    clienteSmtp.Credentials = new NetworkCredential("dgomezmart@uoc.edu", "Manonegra-123"); 
                    clienteSmtp.EnableSsl = true;
                    clienteSmtp.Send(mensaje);
                }
            }
        }

        private string ConstruirCuerpoHTML(string nombreUsuario, string token)
        {
            string logoUrl = "url-de-tu-logo.jpg"; 

            StringBuilder cuerpo = new StringBuilder();
            cuerpo.AppendLine("<html>");
            cuerpo.AppendLine("<head>");
            cuerpo.AppendLine("<style>");
            cuerpo.AppendLine("/* Agrega estilos CSS según tus necesidades */");
            cuerpo.AppendLine("</style>");
            cuerpo.AppendLine("</head>");
            cuerpo.AppendLine("<body>");
            cuerpo.AppendLine("<div id='header'>");
            cuerpo.AppendLine("<img src='" + logoUrl + "' alt='Logo' />");
            cuerpo.AppendLine("<h1>Bienvenido a Nuestra Aplicación</h1>");
            cuerpo.AppendLine("</div>");
            cuerpo.AppendLine("<div id='cuerpo'>");
            cuerpo.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
            cuerpo.AppendLine("<p>Gracias por unirte a nuestra aplicación.</p>");
            cuerpo.AppendLine("<p>Haz clic en el siguiente botón para activar tu cuenta:</p>");
            cuerpo.AppendLine("<a href='https://tuservidor.com/activar-cuenta?token=" + token + "'>");
            cuerpo.AppendLine("<button type='button'>Activar Cuenta</button>");
            cuerpo.AppendLine("</a>");
            cuerpo.AppendLine("</div>");
            cuerpo.AppendLine("</body>");
            cuerpo.AppendLine("</html>");

            return cuerpo.ToString();
        } 
    }

    [Route("[controller]")]
    [ApiController]
    public class CorreoController : ControllerBase
    {
        private readonly CorreoService _correoService;
        private readonly AppConfiguration _appConfiguration;

        public CorreoController(CorreoService correoService,
                                IOptions<AppConfiguration> options) {
            _correoService = correoService ?? throw new ArgumentNullException(nameof(correoService));
            _appConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
        }
         
        [HttpPost("Enviar")]
        public IActionResult EnviarCorreo([FromBody] CorreoRequest request) {
            try {
                _correoService.EnviarCorreo(request.Destinatario, request.Asunto, request.Cuerpo, _appConfiguration.ServidorSmtp, _appConfiguration.PuertoSmtp, _appConfiguration.UsuarioSmtp, _appConfiguration.ContraseñaSmtp);
                return Ok("Correo enviado correctamente");
            }
            catch (Exception ex) {
                return BadRequest($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}