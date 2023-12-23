using System.Net.Mail;
using System.Net;
using System.Text;
using API.Requests;

namespace API.Services
{
    //
    public enum TipoEnvioCorreos {
        ValidaciónCuenta,// registro
        Bienvenida, // cuenta validada
        SuscripciónActivada,
        CambiarContraseña, // TODO: ¿Es Necesario?
        ContraseñaCambiada,
        ReservaProducto,
        InscripcionActividad
    }

    //
    public class ContenidoCorreo {

        public ContenidoCorreo() { }

        public ContenidoCorreo(string asunto, string body) {
            Asunto = asunto;
            Cuerpo = body;
        }

        public string Asunto { get; set; }
        public string? Cuerpo { get; set; }
    }

    //
    public class CorreoService {

        public void EnviarCorreo(CorreoRequest request, string servidorSmtp, int puertoSmtp, string usuarioSmtp, string contraseñaSmtp)
        {
            var contenido = ConstruirCuerpoHTML(request.TipoEnvio, "nombreUsuario", request.Destinatario);
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
                    clienteSmtp.Send(mensaje);
                }
            }
        } 

        private ContenidoCorreo ConstruirCuerpoHTML(TipoEnvioCorreos tipoEnvio, string nombreUsuario, string email)
        {
            string asunto = string.Empty;
            string logoUrl = "https://www.getautismactive.com/wp-content/uploads/2021/01/Test-Logo-Circle-black-transparent.png";

            StringBuilder body = new();
            switch (tipoEnvio) {
                case TipoEnvioCorreos.ValidaciónCuenta: 
                    asunto = "Valida tu nueva cuenta";
                    body = BuildBodyValidateAccount(logoUrl, nombreUsuario, email);
                    break;
                case TipoEnvioCorreos.Bienvenida: 
                    asunto = "Bienvenidx a nuestra aplicación";
                    body = BuildBodyWelcome(logoUrl, nombreUsuario, email);
                    break;
                case TipoEnvioCorreos.CambiarContraseña:

                    break;
                case TipoEnvioCorreos.ContraseñaCambiada:

                    asunto = "Tu contraseña ha cambiado";
                    body = BuildBodyPasswordChanged(logoUrl, nombreUsuario, email);


                    break;
                case TipoEnvioCorreos.SuscripciónActivada:

                    break;
            } 

            return new ContenidoCorreo(asunto, body.ToString());
        }

        private StringBuilder BuildBodyPasswordChanged(string logo, string nombreUsuario, string email)
        {
            StringBuilder body = new();

            body.AppendLine("<html>");
            body.AppendLine("<head>");
            body.AppendLine("<style>");
            body.AppendLine("/* Estilos CSS */");
            body.AppendLine("</style>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine("<div id='header'>");
            body.AppendLine("<img width='100px' src='" + logo + "' alt='Logo' />");
            body.AppendLine("<h1>Contraseña modificada</h1>");
            body.AppendLine("</div>");
            body.AppendLine("<div id='cuerpo'>");
            body.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
            body.AppendLine("<p>La contraseña de tu cuenta se ha cambiado correctamente.</p>");
            body.AppendLine("<p>Haz clic en el siguiente botón para ir al inicio de sesión:</p>");
            body.AppendLine("<a href='https://localhost:7175/WebPages/login.html?email=" + email + "'>");
            body.AppendLine("<button type='button'>INICIAR SESIÓN</button>");
            body.AppendLine("</a>");
            body.AppendLine("</div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body;
        }

        private StringBuilder BuildBodyValidateAccount(string logo, string nombreUsuario, string email)
        {
            StringBuilder body = new();

            body.AppendLine("<html>");
            body.AppendLine("<head>");
            body.AppendLine("<style>");
            body.AppendLine("/* Estilos CSS */");
            body.AppendLine("</style>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine("<div id='header'>");
            body.AppendLine("<img width='100px' src='" + logo + "' alt='Logo' />");
            body.AppendLine("<h1>Activa tu cuenta</h1>");
            body.AppendLine("</div>");
            body.AppendLine("<div id='cuerpo'>");
            body.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
            body.AppendLine("<p>Gracias por unirte a nuestra aplicación.</p>");
            body.AppendLine("<p>Haz clic en el siguiente botón para activar tu cuenta:</p>");
            body.AppendLine("<a href='https://localhost:7175/WebPages/validateAccount.html?email=" + email + "'>");
            body.AppendLine("<button type='button'>Activar Cuenta</button>");
            body.AppendLine("</a>");
            body.AppendLine("</div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body;
        }

        private StringBuilder BuildBodyWelcome(string logo, string nombreUsuario, string email)
        {
            StringBuilder body = new();

            body.AppendLine("<html>");
            body.AppendLine("<head>");
            body.AppendLine("<style>");
            body.AppendLine("/* Estilos CSS */");
            body.AppendLine("</style>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine("<div id='header'>");
            body.AppendLine("<img width='100px' src='" + logo + "' alt='Logo' />");
            body.AppendLine("<h1>Bienvenido a nuestra plataforma</h1>");
            body.AppendLine("</div>");
            body.AppendLine("<div id='cuerpo'>");
            body.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
            body.AppendLine("<p>Gracias por unirte a nuestra aplicación.</p>");
            body.AppendLine("<p>Inicia sesión para explorar tus beneficios como vecinx.</p>");
            body.AppendLine("<a href='https://localhost:7175/WebPages/index.html'>");
            body.AppendLine("<button type='button'>Ir a la platforma</button>");
            body.AppendLine("</a>");
            body.AppendLine("</div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body;
        }
    }
}