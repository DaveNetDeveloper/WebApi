using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(ILogger<ProductosController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerProducto/{id}")]
        public IActionResult ObtenerProductoById(int id)
        {
            var productDb = _dbContext.Productos.Find(id);
            return productDb != null ? Ok(productDb) : NoContent();
        }

        [HttpGet("ObtenerProductoByName")]
        public IActionResult ObtenerProductoByName([FromQuery] string nombre)
        {
            var productDb = _dbContext.Productos
            .Where(r => r.nombre.ToLower() == nombre.ToLower())
            .ToList();

            return productDb != null ? Ok(productDb) : NoContent();
        }

        [HttpGet("ObtenerProductos")]
        public IActionResult ObtenerProductos()
        {
            var productosDbList = _dbContext.Productos.ToList();
            return (productosDbList != null && productosDbList.Count > 0) ? Ok(productosDbList) : NoContent();
        }

        [HttpPost("CrearProducto")]
        public IActionResult CrearProducto([FromBody] Producto producto)
        {
            var nuevoProducto = new Producto {
                id = _dbContext.Productos.Count()+1,
                idEntidad = producto.idEntidad,
                nombre = producto.nombre,
                descripción = producto.descripción,
                puntos = producto.puntos,
                activo = producto.activo,
                precio = producto.precio,
                popularidad = producto.popularidad,
                descripcionCorta = producto.descripcionCorta,
                disponible = producto.disponible,
                informacioExtra = producto.informacioExtra,
                linkInstagram = producto.linkInstagram,
                linkFacebook = producto.linkFacebook,
                linkYoutube = producto.linkYoutube
            };

            _dbContext.Productos.Add(nuevoProducto);
            _dbContext.SaveChanges(); 

            return Ok(nuevoProducto);
        } 

        [HttpPut("ActualizarProducto")]
        public IActionResult ActualizarProducto([FromBody] Producto producto)
        {
            var productoDb = _dbContext.Productos
           .Where(r => r.id == producto.id)
           .SingleOrDefault();

            if (productoDb != null) {
                productoDb.idEntidad = producto.idEntidad;
                productoDb.nombre = producto.nombre;
                productoDb.descripción = producto.descripción;
                productoDb.puntos = producto.puntos;
                productoDb.activo = producto.activo;
                productoDb.precio = producto.precio;
                productoDb.popularidad = producto.popularidad;
                productoDb.descripcionCorta = producto.descripcionCorta;
                productoDb.disponible = producto.disponible;
                productoDb.informacioExtra = producto.informacioExtra;
                productoDb.linkInstagram = producto.linkInstagram;
                productoDb.linkFacebook = producto.linkFacebook;
                productoDb.linkYoutube = producto.linkYoutube;

                _dbContext.SaveChanges();
                return Ok(productoDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var productDb = _dbContext.Productos
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (productDb != null) {
                _dbContext.Productos.Remove(productDb);
                _dbContext.SaveChanges();
                return Ok($"El producto [{productDb.nombre}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}