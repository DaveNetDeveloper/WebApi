using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RolesController> _logger;

        public RolesController(ILogger<RolesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerRol/{id}")]
        public IActionResult ObtenerRolById(Guid id)
        {
            var rolDb = _dbContext.Roles.Find(id);
            return rolDb != null ? Ok(rolDb) : NoContent();
        }

        [HttpGet("ObtenerRolByName")]
        public IActionResult ObtenerRolByName([FromQuery] string nombre)
        {
            var rolDb = _dbContext.Roles
            .Where(r => r.nombre.ToLower() == nombre.ToLower())
            .SingleOrDefault();

            return rolDb != null ? Ok(rolDb) : NoContent();
        }

        [HttpGet("ObtenerRoles")]
        public IActionResult ObtenerRoles()
        {
            var rolesDbList = _dbContext.Roles.ToList();
            return (rolesDbList != null && rolesDbList.Count > 0) ? Ok(rolesDbList) : NoContent();
        }

        [HttpPost("CrearRol")]
        public IActionResult CrearRol([FromBody] Rol rol)
        {
            var nuevoRol = new Rol {
                id = new Guid(),
                nombre = rol.nombre,
                descripcion = rol.descripcion
            };
            _dbContext.Roles.Add(nuevoRol);
            _dbContext.SaveChanges();
            return Ok($"Rol creado correctamente: {nuevoRol.nombre}.");
        }

        [HttpPut("ActualizarRol")]
        public IActionResult ActualizarRol([FromBody] Rol rol)
        {
            var rolDb = _dbContext.Roles
           .Where(r => r.id == rol.id)
           .SingleOrDefault();

            if (rolDb != null) {
                rolDb.id = rol.id;
                rolDb.nombre = rol.nombre;
                rolDb.descripcion = rol.descripcion;
                _dbContext.SaveChanges();
                return Ok($"Rol modificado correctamente: {rolDb.nombre}.");
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(Guid id)
        {
            var rolDb = _dbContext.Roles
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (rolDb != null) {
                _dbContext.Roles.Remove(rolDb);
                _dbContext.SaveChanges();
                return Ok($"El rol [{rolDb.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}