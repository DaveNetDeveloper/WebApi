using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FAQSController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<FAQSController> _logger;

        public FAQSController(ILogger<FAQSController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerFAQ/{id}")]
        public IActionResult ObtenerFAQById(Guid id)
        {
            var FAQDb = _dbContext.FAQS.Find(id);
            return FAQDb != null ? Ok(FAQDb) : NoContent();
        }

        [HttpGet("ObtenerFAQS")]
        public IActionResult ObtenerFAQS()
        {
            var FAQSDbList = _dbContext.FAQS.ToList();
            return (FAQSDbList != null && FAQSDbList.Count > 0) ? Ok(FAQSDbList) : NoContent();
        }

        [HttpPost("CrearFAQ")]
        public IActionResult CrearFAQ([FromBody] FAQ FAQ)
        {
            var nuevaFAQ = new FAQ {
                id = new Guid(),
                orden = FAQ.orden,
                pregunta = FAQ.pregunta,
                respuesta = FAQ.respuesta
            };

            _dbContext.FAQS.Add(nuevaFAQ);
            _dbContext.SaveChanges(); 

            return Ok(nuevaFAQ);
        } 

        [HttpPut("ActualizarFAQ")]
        public IActionResult ActualizarFAQ([FromBody] FAQ FAQ)
        {
            var FAQDb = _dbContext.FAQS
           .Where(r => r.id == FAQ.id)
           .SingleOrDefault();

            if (FAQDb != null) {
                FAQDb.orden = FAQ.orden;
                FAQDb.pregunta = FAQ.pregunta;
                FAQDb.respuesta = FAQ.respuesta;

                _dbContext.SaveChanges();
                return Ok(FAQDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(Guid id)
        {
            var FAQDb = _dbContext.FAQS
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (FAQDb != null) {
                _dbContext.FAQS.Remove(FAQDb);
                _dbContext.SaveChanges();
                return Ok($"La FAQ [{FAQDb.id}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}