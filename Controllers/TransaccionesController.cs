using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransaccionesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TransaccionesController> _logger;

        public TransaccionesController(ILogger<TransaccionesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerTransaccion/{id}")]
        public IActionResult ObtenerTransaccionById(int id)
        {
            var transaccionDb = _dbContext.Transacciones.Find(id);
            return transaccionDb != null ? Ok(transaccionDb) : NoContent();
        }

        [HttpGet("ObtenerTransaccionByName")]
        public IActionResult ObtenerTransaccionByName([FromQuery] string nombre)
        {
            var transaccionDb = _dbContext.Transacciones
            .Where(r => r.nombre.ToLower() == nombre.ToLower())
            .ToList();

            return transaccionDb != null ? Ok(transaccionDb) : NoContent();
        }

        [HttpGet("ObtenerTransacciones")]
        public IActionResult ObtenerTransacciones()
        {
            var transaccionesDbList = _dbContext.Transacciones.ToList();
            return (transaccionesDbList != null && transaccionesDbList.Count > 0) ? Ok(transaccionesDbList) : NoContent();
        }

        [HttpPost("CrearTransaccion")]
        public IActionResult CrearTransaccion([FromBody] Transaccion transaccion)
        {
            var nuevaTransaccion = new Transaccion {
                id = _dbContext.Transacciones.Count()+1,
                nombre = transaccion.nombre,
                puntos = transaccion.puntos,
                fecha = DateTime.UtcNow,
                idProducto = transaccion.idProducto,
                idUsuario = transaccion.idUsuario
            };
            _dbContext.Transacciones.Add(nuevaTransaccion);
            _dbContext.SaveChanges();

            ActualizarBalance(nuevaTransaccion.idUsuario, nuevaTransaccion.puntos);

            return Ok(nuevaTransaccion);
        }

        private void ActualizarBalance(int idUsuario, int puntosTransaccion)
        { 
            var usuarioDb = _dbContext.Usuarios
           .Where(u => u.id == idUsuario)
           .SingleOrDefault();

            if (usuarioDb != null) {
                usuarioDb.puntos += puntosTransaccion;
                _dbContext.SaveChanges(); 
            }
        }

        [HttpPut("ActualizarTransaccion")]
        public IActionResult ActualizarTransaccion([FromBody] Transaccion transaccion)
        {
            var transaccionDb = _dbContext.Transacciones
           .Where(r => r.id == transaccion.id)
           .SingleOrDefault();

            if (transaccionDb != null) { 
                transaccionDb.nombre = transaccion.nombre;
                transaccionDb.puntos = transaccion.puntos;
                transaccionDb.fecha = transaccion.fecha;
                transaccionDb.idProducto = transaccion.idProducto;
                transaccionDb.idUsuario = transaccion.idUsuario;

                _dbContext.SaveChanges();
                return Ok(transaccionDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var transaccionDb = _dbContext.Transacciones
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (transaccionDb != null) {
                _dbContext.Transacciones.Remove(transaccionDb);
                _dbContext.SaveChanges();
                return Ok($"La Transaccion [{transaccionDb.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}