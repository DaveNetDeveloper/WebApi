using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestimoniosController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TestimoniosController> _logger;

        public TestimoniosController(ILogger<TestimoniosController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerTestimonio/{id}")]
        public IActionResult ObtenerTestimonioById(int id)
        {
            var testimonioDb = _dbContext.Testimonios.Find(id);
            return testimonioDb != null ? Ok(testimonioDb) : NoContent();
        }

        [HttpGet("ObtenerTestimonios")]
        public IActionResult ObtenerTestimonios()
        {
            var testimonioDbList = _dbContext.Testimonios.ToList();
            return (testimonioDbList != null && testimonioDbList.Count > 0) ? Ok(testimonioDbList) : NoContent();
        }

        [HttpPost("CrearTestimonio")]
        public IActionResult CrearTestimonio([FromBody] Testimonio testimonio)
        {
            var nuevoTestimonio = new Testimonio {
                id = _dbContext.Testimonios.Count() + 1,
                nombreUsuario = testimonio.nombreUsuario,
                texto = testimonio.texto,
                imagen = testimonio.imagen,
                fecha = testimonio.fecha
            };

            _dbContext.Testimonios.Add(nuevoTestimonio);
            _dbContext.SaveChanges(); 

            return Ok(nuevoTestimonio);
        } 

        [HttpPut("ActualizarTestimonio")]
        public IActionResult ActualizarTestimonio([FromBody] Testimonio testimonio)
        {
            var testimonioDb = _dbContext.Testimonios
           .Where(r => r.id == testimonio.id)
           .SingleOrDefault();

            if (testimonioDb != null) {
                testimonioDb.nombreUsuario = testimonio.nombreUsuario;
                testimonioDb.texto = testimonio.texto;
                testimonioDb.imagen = testimonio.imagen;
                testimonioDb.fecha = testimonio.fecha;

                _dbContext.SaveChanges();
                return Ok(testimonioDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var testimonioDb = _dbContext.Testimonios
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (testimonioDb != null) {
                _dbContext.Testimonios.Remove(testimonioDb);
                _dbContext.SaveChanges();
                return Ok($"El testimonio [{testimonioDb.id}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}