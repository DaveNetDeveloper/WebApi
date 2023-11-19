using API.Models;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;

using API.Utils; 

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UsuariosController> _logger;
        private readonly int defaultPuntos = 300;

        public UsuariosController(ILogger<UsuariosController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        #region [private functions]

        private bool ValidarToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var usuarioDb = _dbContext.Usuarios
                        .Where(u => u.token == token)
                        .SingleOrDefault();
                if (usuarioDb == null) return false;

                var expiracionToken = usuarioDb.expiracionToken;
                if (expiracionToken > DateTime.UtcNow)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region [web services]

        //[Authorize]
        [HttpGet("ObtenerUsuario/{id}")]
        public IActionResult ObtenerUsuarioById(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString(); // esto puede ser una propiedad del controller?
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized(new { mensaje = "Token no informado." });
            if (!ValidarToken(token)) return Unauthorized(new { mensaje = "Token no válido." });

            var usuarioDb = _dbContext.Usuarios.Find(id); 
            return usuarioDb != null ? Ok(usuarioDb) : NoContent();
        }

        [HttpGet("ObtenerUsuarioByEmail")]
        public IActionResult ObtenerUsuarioByEmail([FromQuery] string email)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString(); // esto puede ser una propiedad del controller?
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized(new { mensaje = "Token no informado." });
            if (!ValidarToken(token)) return Unauthorized(new { mensaje = "Token no válido." });

            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower())
            .SingleOrDefault();

            return usuarioDb != null ? Ok(usuarioDb) : NoContent();
        }

        [HttpGet("ObtenerUsuarios")]
        public IActionResult ObtenerUsuarios()
        {
            var usersList = _dbContext.Usuarios.ToList();
            return (usersList != null && usersList.Count > 0) ? Ok(usersList) : NoContent();
        }

        [HttpPost("CrearUsuario")]
        public IActionResult CrearUsuario([FromBody] Usuario usuario)
        {
            var nuevoUsuario = new Usuario {
                id = _dbContext.Usuarios.Count() + 1,
                nombre = usuario.nombre,
                correo = usuario.correo,
                apellidos = usuario.apellidos,
                activo = usuario.activo,
                contraseña = usuario.contraseña,
                fechaNacimiento = usuario.fechaNacimiento.ToUniversalTime(),
                suscrito = usuario.suscrito,
                fechaCreación = DateTime.UtcNow,
                ultimaConexion = null,
                puntos = defaultPuntos,
                token= null,
                expiracionToken = null
            };

            _dbContext.Usuarios.Add(nuevoUsuario);
            _dbContext.SaveChanges();
            return Ok($"Usuario creado correctamente: {nuevoUsuario.nombre}");
        }

        [HttpPut("ActualizarUsuario")]
        public IActionResult ActualizarUsuario([FromBody] Usuario usuario) 
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString(); // esto puede ser una propiedad del controller?
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized(new { mensaje = "Token no informado." });
            if (!ValidarToken(token)) return Unauthorized(new { mensaje = "Token no válido." });

            var usuarioDb = _dbContext.Usuarios
           .Where(u => u.id == usuario.id)
           .SingleOrDefault();

            if (usuarioDb != null) {
                usuarioDb.nombre = usuario.nombre;
                usuarioDb.correo = usuario.correo;
                usuarioDb.apellidos = usuario.apellidos;
                usuarioDb.activo = usuario.activo;
                usuarioDb.fechaNacimiento = usuario.fechaNacimiento.ToUniversalTime();
                usuarioDb.suscrito = usuario.suscrito;
                usuarioDb.puntos = usuario.puntos;

                _dbContext.SaveChanges();
                return Ok($"Usuario modificado correctamente: {usuarioDb.nombre}");
            }
            else {
                return NotFound();
            } 
        }
        
        [HttpPatch("Login")]
        public IActionResult Login([FromQuery] string email, string contraseña)
        {
            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower() && u.contraseña.ToLower() == contraseña.ToLower())
            .SingleOrDefault();

            if (usuarioDb != null) {
                
                usuarioDb.ultimaConexion = DateTime.UtcNow;
                usuarioDb.token = "Bearer " + Utilities.GetHashedValue(DateTime.Now.ToString());
                usuarioDb.expiracionToken = DateTime.Now.AddDays(30).ToUniversalTime();
                _dbContext.SaveChanges();

                // TODO en front llamar despues el envio de mail al usuario con el link de validación de la nueva cuenta
                 
                //return Ok($"Usuario {usuario.nombre} logeado correctamente.");
                return Ok(usuarioDb);
            }
            else {
                return NoContent();
            }
        }

        [HttpPatch("CambiarContraseña")]
        public IActionResult CambiarContraseña([FromQuery] string email, string nuevaContraseña) 
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString(); // esto puede ser una propiedad del controller?
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized(new { mensaje = "Token no informado." });
            if (!ValidarToken(token)) return Unauthorized(new { mensaje = "Token no válido." });

            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower())
            .SingleOrDefault();

            if (usuarioDb != null) {
                usuarioDb.contraseña = nuevaContraseña;
                _dbContext.SaveChanges();

                // TODO en front llamar despues el envio de mail al usuario con el link de cambio de contraseña

                return Ok($"Contraseña cambiada correctamente para el usuario con el correo [{usuarioDb.correo}].");
            }
            else {
                return NotFound();
            }
        }

        [HttpPatch("ValidarCuenta")]
        public IActionResult ValidarCuenta([FromQuery] string email) 
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString(); // esto puede ser una propiedad del controller?
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized(new { mensaje = "Token no informado." });
            if (!ValidarToken(token)) return Unauthorized(new { mensaje = "Token no válido." });

            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower())
            .SingleOrDefault();

            if (usuarioDb != null) {
                usuarioDb.activo = true;
                
                usuarioDb.ultimaConexion = DateTime.UtcNow;
                usuarioDb.token = "Bearer " + Utilities.GetHashedValue(DateTime.Now.ToString());
                usuarioDb.expiracionToken = DateTime.Now.AddDays(30).ToUniversalTime();

                _dbContext.SaveChanges();
                return Ok($"El usuario [{usuarioDb.correo}] se ha activado correctamente.");
            }
            else {
                return NotFound();
            }
        }

        [HttpPatch("ActivacionSuscripcion")]
        public IActionResult ActivacionSuscripcion([FromQuery] string email, bool suscrito) 
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString(); // esto puede ser una propiedad del controller?
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized(new { mensaje = "Token no informado." });
            if (!ValidarToken(token)) return Unauthorized(new { mensaje = "Token no válido." });

            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower())
            .SingleOrDefault();

            if (usuarioDb != null) {
                usuarioDb.suscrito = suscrito;
                _dbContext.SaveChanges();
                return Ok($"La suscripción del usuario [{usuarioDb.correo}] se ha modificado correctamente.");
            }
            else {
                return NotFound();
            }
        }

        //[Authorize]
        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString(); // esto puede ser una propiedad del controller?
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized(new { mensaje = "Token no informado." });
            if (!ValidarToken(token)) return Unauthorized(new { mensaje = "Token no válido." });

            var usuarioDb = _dbContext.Usuarios
           .Where(u => u.id == id)
           .SingleOrDefault();

            if (usuarioDb != null) {
                _dbContext.Usuarios.Remove(usuarioDb);
                _dbContext.SaveChanges();
                return Ok($"El usuario [{usuarioDb.correo}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }

        #endregion
    }
}