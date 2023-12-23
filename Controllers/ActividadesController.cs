using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActividadesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ActividadesController> _logger;

        public ActividadesController(ILogger<ActividadesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerActividad/{id}")]
        public IActionResult ObtenerActividadById(int id)
        {
            var actividadDb = _dbContext.Actividades.Find(id);
            return actividadDb != null ? Ok(actividadDb) : NoContent();
        }

        [HttpGet("ObtenerActividadByName")]
        public IActionResult ObtenerActividadByName([FromQuery] string nombre)
        {
            var actividadDb = _dbContext.Actividades
            .Where(r => r.nombre.ToLower() == nombre.ToLower())
            .ToList();

            return actividadDb != null ? Ok(actividadDb) : NoContent();
        }

        [HttpGet("ObtenerActividades")]
        public IActionResult ObtenerActividades()
        {
            var actividadDbList = _dbContext.Actividades.ToList();
            return (actividadDbList != null && actividadDbList.Count > 0) ? Ok(actividadDbList) : NoContent();
        }

        [HttpPost("CrearActividad")]
        public IActionResult CrearActividad([FromBody] Actividad actividad)
        {
            var nuevaActividad = new Actividad {
                id = _dbContext.Actividades.Count()+1,
                idEntidad = actividad.idEntidad,
                nombre = actividad.nombre,
                descripcion = actividad.descripcion,
                linkEvento = actividad.linkEvento,
                idTipoActividad = actividad.idTipoActividad,
                ubicación = actividad.ubicación,
                popularidad = actividad.popularidad,
                descripcionCorta = actividad.descripcionCorta,
                fechaInicio = actividad.fechaInicio,
                fechaFin = actividad.fechaFin,
                gratis = actividad.gratis,
                activo = actividad.activo,
                informacioExtra = actividad.informacioExtra,
                linkInstagram = actividad.linkInstagram,
                linkFacebook = actividad.linkFacebook,
                linkYoutube = actividad.linkYoutube
            };

            _dbContext.Actividades.Add(nuevaActividad);
            _dbContext.SaveChanges(); 

            return Ok(nuevaActividad);
        } 

        [HttpPut("ActualizarActividad")]
        public IActionResult ActualizarActividad([FromBody] Actividad actividad)
        {
            var actividadDb = _dbContext.Actividades
           .Where(r => r.id == actividad.id)
           .SingleOrDefault();

            if (actividadDb != null) {
                actividadDb.idEntidad = actividad.idEntidad;
                actividadDb.nombre = actividad.nombre;
                actividadDb.descripcion = actividad.descripcion;
                actividadDb.linkEvento = actividad.linkEvento;
                actividadDb.idTipoActividad = actividad.idTipoActividad;
                actividadDb.ubicación = actividad.ubicación;
                actividadDb.popularidad = actividad.popularidad;
                actividadDb.descripcionCorta = actividad.descripcionCorta;
                actividadDb.fechaInicio = actividad.fechaInicio;
                actividadDb.fechaFin = actividad.fechaFin;
                actividadDb.gratis = actividad.gratis;
                actividadDb.activo = actividad.activo;
                actividadDb.informacioExtra = actividad.informacioExtra;
                actividadDb.linkInstagram = actividad.linkInstagram;
                actividadDb.linkFacebook = actividad.linkFacebook;
                actividadDb.linkYoutube = actividad.linkYoutube;

                _dbContext.SaveChanges();
                return Ok(actividadDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var actividadDb = _dbContext.Actividades
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (actividadDb != null) {
                _dbContext.Actividades.Remove(actividadDb);
                _dbContext.SaveChanges();
                return Ok($"La actividad [{actividadDb.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}