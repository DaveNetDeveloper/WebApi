using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoEntidadesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TipoEntidadesController> _logger;

        public TipoEntidadesController(ILogger<TipoEntidadesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerTipoEntidad/{id}")]
        public IActionResult ObtenerTipoEntidadById(Guid id)
        {
            var dbEntityType = _dbContext.TipoEntidades.Find(id);
            return dbEntityType != null ? Ok(dbEntityType) : NoContent();
        }

        [HttpGet("ObtenerTipoEntidadByName")]
        public IActionResult ObtenerTipoEntidadByname([FromQuery] string nombre)
        {
            var dbEntityType = _dbContext.TipoEntidades
            .Where(e => e.nombre.ToLower() == nombre.ToLower())
            .SingleOrDefault();

            return dbEntityType != null ? Ok(dbEntityType) : NoContent();
        }

        [HttpGet("ObtenerTipoEntidades")]
        public IActionResult ObtenerTipoEntidades()
        {
            var dbList = _dbContext.TipoEntidades.ToList();
            return (dbList != null && dbList.Count > 0) ? Ok(dbList) : NoContent();
        }

        [HttpPost("CrearTipoEntidad")]
        public IActionResult CrearTipoEntidad([FromBody] TipoEntidad tipoEntidad)
        {
            var nuevoTipoEntidad = new TipoEntidad
            {
                id = new Guid(),
                nombre = tipoEntidad.nombre,
                descripcion = tipoEntidad.descripcion
            };
            _dbContext.TipoEntidades.Add(nuevoTipoEntidad);
            _dbContext.SaveChanges();
            return Ok($"Tipo de entidad creado correctamente: {nuevoTipoEntidad.nombre}.");
        }

        [HttpPut("ActualizarTipoEntidad")]
        public IActionResult ActualizarTipoEntidad([FromBody] TipoEntidad tipoEntidad)
        {
            var dbEntityType = _dbContext.TipoEntidades
           .Where(e => e.id == tipoEntidad.id)
           .SingleOrDefault();

            if (dbEntityType != null) {
                dbEntityType.nombre = tipoEntidad.nombre;
                dbEntityType.descripcion = tipoEntidad.descripcion;
                _dbContext.SaveChanges();
                return Ok($"Tipo de entidad modificado correctamente: {dbEntityType.nombre}.");
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(Guid id)
        {
            var dbEntityType = _dbContext.TipoEntidades
           .Where(e => e.id == id)
           .SingleOrDefault();

            if (dbEntityType != null) {
                _dbContext.TipoEntidades.Remove(dbEntityType);
                _dbContext.SaveChanges();
                return Ok($"El tipo de entidad [{dbEntityType.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}