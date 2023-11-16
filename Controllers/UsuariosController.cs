using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        /*private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };*/

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ILogger<UsuariosController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        //[Route("[controller]/[action]/idUsuario")]
        [HttpGet("ObtenerUsuario/{id}")]
        public IActionResult ObtenerUsuarioById(int id)
        {
            var usuarioDb = _dbContext.Usuarios.Find(id);
            return usuarioDb != null ? Ok(usuarioDb) : NoContent();
        }

        [HttpGet("ObtenerUsuarioByEmail")]
        public IActionResult ObtenerUsuarioByEmail([FromQuery] string email)
        {
            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower())
            .SingleOrDefault();

            return usuarioDb != null ? Ok(usuarioDb) : NoContent();
        }

        //[Route("[controller]/[action]/)]
        [HttpGet("ObtenerUsuarios")]
        public IActionResult ObtenerUsuarios()
        {
            var usersList = _dbContext.Usuarios.ToList();
            return (usersList != null && usersList.Count > 0) ? Ok(usersList) : NoContent();
        }

        //[Route("[controller]")]
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
                ultimaconexion = null,
                puntos = 0

            };

            _dbContext.Usuarios.Add(nuevoUsuario);
            _dbContext.SaveChanges();
            return Ok($"Usuario creado correctamente: {nuevoUsuario.nombre}");
        }

        [HttpPut("ActualizarUsuario")]
        public IActionResult ActualizarUsuario([FromBody] Usuario usuario) 
        {
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
                usuarioDb.ultimaconexion = DateTime.UtcNow;
                _dbContext.SaveChanges();
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
            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower())
            .SingleOrDefault();

            if (usuarioDb != null) {
                usuarioDb.contraseña = nuevaContraseña;
                _dbContext.SaveChanges();
                return Ok($"Contraseña cambiada correctamente para el usuario con el correo [{usuarioDb.correo}].");
            }
            else {
                return NotFound();
            }
        }

        [HttpPatch("ValidarCuenta")]
        public IActionResult ValidarCuenta([FromQuery] string email) 
        {
            var usuarioDb = _dbContext.Usuarios
            .Where(u => u.correo.ToLower() == email.ToLower())
            .SingleOrDefault();

            if (usuarioDb != null) {

                usuarioDb.activo = true;
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

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
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
    }
}