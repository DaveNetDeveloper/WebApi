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
            var dbEntity = _dbContext.TipoEntidades.Find(id);
            return dbEntity != null ? Ok(dbEntity) : NoContent();
        }

        [HttpGet("ObtenerTipoEntidadByName")]
        public IActionResult ObtenerTipoEntidadByname([FromQuery] string nombre)
        {
            var dbEntity = _dbContext.TipoEntidades
            .Where(e => e.nombre.ToLower() == nombre.ToLower())
            .SingleOrDefault();

            return dbEntity != null ? Ok(dbEntity) : NoContent();
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
            var dbEntity = _dbContext.TipoEntidades
           .Where(e => e.id == tipoEntidad.id)
           .SingleOrDefault();

            if (dbEntity != null) { 
                dbEntity.nombre = tipoEntidad.nombre;
                dbEntity.descripcion = tipoEntidad.descripcion;
                _dbContext.SaveChanges();
                return Ok($"Tipo de entidad modificado correctamente: {dbEntity.nombre}.");
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(Guid id)
        {
            var dbEntity = _dbContext.TipoEntidades
           .Where(e => e.id == id)
           .SingleOrDefault();

            if (dbEntity != null) {
                _dbContext.TipoEntidades.Remove(dbEntity);
                _dbContext.SaveChanges();
                return Ok($"El tipo de entidad [{dbEntity.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}