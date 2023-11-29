using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntidadesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<EntidadesController> _logger;

        public EntidadesController(ILogger<EntidadesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerEntidad/{id}")]
        public IActionResult ObtenerEntidadById(int id)
        {
            var entidadDb = _dbContext.Entidades.Find(id);
            return entidadDb != null ? Ok(entidadDb) : NoContent();
        }

        [HttpGet("ObtenerEntidadByName")]
        public IActionResult ObtenerEntidadByName([FromQuery] string nombre)
        {
            var entidadDb = _dbContext.Entidades
            .Where(r => r.nombre.ToLower() == nombre.ToLower())
            .ToList();

            return entidadDb != null ? Ok(entidadDb) : NoContent();
        }

        [HttpGet("ObtenerEntidades")]
        public IActionResult ObtenerEntidades()
        {
            var entidadesDbList = _dbContext.Entidades.ToList();
            return (entidadesDbList != null && entidadesDbList.Count > 0) ? Ok(entidadesDbList) : NoContent();
        }

        [HttpPost("CrearEntidad")]
        public IActionResult CrearEntidad([FromBody] Entidad entidad)
        {
            var nuevaEntidad = new Entidad {
                id = _dbContext.Entidades.Count()+1,
                nombre = entidad.nombre,
                ubicacion = entidad.ubicacion,
                fechaAlta = DateTime.UtcNow,
                popularidad = entidad.popularidad,
                descripcion = entidad.descripcion,
                activo = entidad.activo,
                idTipoEntidad = entidad.idTipoEntidad
            };
            _dbContext.Entidades.Add(nuevaEntidad);
            _dbContext.SaveChanges(); 

            return Ok(nuevaEntidad);
        } 

        [HttpPut("ActualizarEntidad")]
        public IActionResult ActualizarEntidad([FromBody] Entidad entidad)
        {
            var entidadDb = _dbContext.Entidades
           .Where(r => r.id == entidad.id)
           .SingleOrDefault();

            if (entidadDb != null) { 
                entidadDb.nombre = entidad.nombre;
                entidadDb.ubicacion = entidad.ubicacion;
                entidadDb.fechaAlta = entidad.fechaAlta;
                entidadDb.popularidad = entidad.popularidad;
                entidadDb.descripcion = entidad.descripcion;
                entidadDb.activo = entidad.activo;
                entidadDb.idTipoEntidad = entidad.idTipoEntidad;

                _dbContext.SaveChanges();
                return Ok(entidadDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var entidadDb = _dbContext.Entidades
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (entidadDb != null) {
                _dbContext.Entidades.Remove(entidadDb);
                _dbContext.SaveChanges();
                return Ok($"La Entidad [{entidadDb.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}