using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ILogger<CategoriasController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerCategoria/{id}")]
        public IActionResult ObtenerCategoriaById(Guid id)
        {
            var categoriaDb = _dbContext.Categorias.Find(id);
            return categoriaDb != null ? Ok(categoriaDb) : NoContent();
        }

        [HttpGet("ObtenerCategoriaByName")]
        public IActionResult ObtenerCategoriaByName([FromQuery] string nombre)
        {
            var categoriaDb = _dbContext.Categorias
            .Where(r => r.nombre.ToLower() == nombre.ToLower())
            .ToList();

            return categoriaDb != null ? Ok(categoriaDb) : NoContent();
        }

        [HttpGet("ObtenerCategorias")]
        public IActionResult ObtenerCategorias()
        {
            var categoriasDbList = _dbContext.Categorias.ToList();
            return (categoriasDbList != null && categoriasDbList.Count > 0) ? Ok(categoriasDbList) : NoContent();
        }

        [HttpPost("CrearCategoria")]
        public IActionResult CrearCategoria([FromBody] Categoria categoria)
        {
            var nuevaCategoria= new Categoria {
                id = new Guid(),
                idTipoEntidad = categoria.idTipoEntidad,
                nombre = categoria.nombre,
                descripcion = categoria.descripcion
            };

            _dbContext.Categorias.Add(nuevaCategoria);
            _dbContext.SaveChanges(); 

            return Ok(nuevaCategoria);
        } 

        [HttpPut("ActualizarCategoria")]
        public IActionResult ActualizarCategoria([FromBody] Categoria categoria)
        {
            var categoriaDb = _dbContext.Categorias
           .Where(r => r.id == categoria.id)
           .SingleOrDefault();

            if (categoriaDb != null) { 
                categoriaDb.nombre = categoria.nombre;
                categoriaDb.descripcion = categoria.descripcion;
                categoriaDb.idTipoEntidad = categoria.idTipoEntidad;

                _dbContext.SaveChanges();
                return Ok(categoriaDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(Guid id)
        {
            var categoriaDb = _dbContext.Categorias
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (categoriaDb != null) {
                _dbContext.Categorias.Remove(categoriaDb);
                _dbContext.SaveChanges();
                return Ok($"La categoria [{categoriaDb.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}