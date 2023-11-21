using System.Net.Mail;
using System.Net;
using System.Text;
using API.Requests;

namespace API.Services
{
    public enum TipoEnvioCorreos {
        ValidaciónCuenta,
        Bienvenida,
        SuscripciónActivada,
        CambiarContraseña,
        ContraseñaCambiada,
        ReservaProducto,
        InscripcionActividad
    }

    // TODO 
    // Crear controlador en API
    // Crear controlador en Web
    // 
    //  

    public class CorreoService {
        public void EnviarCorreo(CorreoRequest request, string servidorSmtp, int puertoSmtp, string usuarioSmtp, string contraseñaSmtp)
        {

            var tipoEnvio = request.TipoEnvio
;
            using (var mensaje = new MailMessage()) {
                mensaje.From = new MailAddress(usuarioSmtp);
                mensaje.To.Add(request.Destinatario);
                mensaje.Subject = request.Asunto;
                mensaje.Body = request.Cuerpo;
                mensaje.IsBodyHtml = true;

                using (var clienteSmtp = new SmtpClient(servidorSmtp, puertoSmtp)) {
                    clienteSmtp.Credentials = new NetworkCredential(usuarioSmtp, contraseñaSmtp);
                    clienteSmtp.EnableSsl = true;
                    //clienteSmtp.Send(mensaje);
                }
            }
        }

        public void EnviarCorreoConPlantilla(TipoEnvioCorreos tipoEnvio, string destinatario, string nombreUsuario, string token, string servidorSmtp, int puertoSmtp, string usuarioSmtp, string contraseñaSmtp)
        {
            string asunto = "Bienvenido a nuestra aplicación";
            string cuerpo = ConstruirCuerpoHTML(nombreUsuario, token);

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
}
