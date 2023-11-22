using System.Net.Mail;
using System.Net;
using System.Text;
using API.Requests;

namespace API.Services
{
    //
    public enum TipoEnvioCorreos {
        ValidaciónCuenta,
        Bienvenida,
        SuscripciónActivada,
        CambiarContraseña,
        ContraseñaCambiada,
        ReservaProducto,
        InscripcionActividad
    }

    //
    public class ContenidoCorreo {

        public ContenidoCorreo() { }

        public ContenidoCorreo(string asunto, string cuerpo) {
            Asunto = asunto;
            Cuerpo = cuerpo;
        }

        public string Asunto { get; set; }
        public string? Cuerpo { get; set; }
    }


    //
    public class CorreoService {

        public void EnviarCorreo(CorreoRequest request, string servidorSmtp, int puertoSmtp, string usuarioSmtp, string contraseñaSmtp)
        {
            var contenido = ConstruirCuerpoHTML(request.TipoEnvio, "nombreUsuario", "token");
;
            using (var mensaje = new MailMessage()) {
                mensaje.From = new MailAddress(usuarioSmtp);
                mensaje.To.Add(request.Destinatario);
                mensaje.Subject = contenido.Asunto;
                mensaje.Body = contenido.Cuerpo;
                mensaje.IsBodyHtml = true;

                using (var clienteSmtp = new SmtpClient(servidorSmtp, puertoSmtp)) {
                    clienteSmtp.Credentials = new NetworkCredential(usuarioSmtp, contraseñaSmtp);
                    clienteSmtp.EnableSsl = true;
                    //clienteSmtp.Send(mensaje);
                }
            }
        } 

        private ContenidoCorreo ConstruirCuerpoHTML(TipoEnvioCorreos tipoEnvio, string nombreUsuario, string token)
        {
            string asunto = "Bienvenido a nuestra aplicación";

            switch (tipoEnvio) {
                case TipoEnvioCorreos.ValidaciónCuenta:

                    break;
                case TipoEnvioCorreos.Bienvenida:

                    break;
                case TipoEnvioCorreos.CambiarContraseña:

                    break;
                case TipoEnvioCorreos.ContraseñaCambiada:

                    break;
                case TipoEnvioCorreos.SuscripciónActivada:

                    break;
            }

            string logoUrl = "url-de-tu-logo.jpg";

            StringBuilder cuerpo = new();
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

            return new ContenidoCorreo(asunto, cuerpo.ToString());
        }
    }
}