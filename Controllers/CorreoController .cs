using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using API.Models;
using API.Requests;
using API.Services;

namespace API.Controllers
{
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
                _correoService.EnviarCorreo(request, _appConfiguration.ServidorSmtp, _appConfiguration.PuertoSmtp, _appConfiguration.UsuarioSmtp, _appConfiguration.ContraseñaSmtp);
                return Ok("Correo enviado correctamente");
            }
            catch (Exception ex) {
                return BadRequest($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}