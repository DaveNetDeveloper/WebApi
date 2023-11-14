using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        /*private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };*/


        private readonly ApplicationDbContext _dbContext;

        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ILogger<UsuariosController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        [HttpGet(Name = "GetUsuario")]
        public IEnumerable<Usuario> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Usuario
            {
                Correo = "", /*DateTime.Now.AddDays(index)*/
                Id = Random.Shared.Next(-20, 55),
                Nombre = "" /*Summaries[Random.Shared.Next(Summaries.Length)]*/
            })
            .ToArray();
        }


        [HttpPost(Name = "CreateUsuario")]
        public IActionResult CrearUsuario([FromBody] Usuario usuario)
        {
            var nuevoUsuario = new Usuario { Id = usuario.Id, Nombre = usuario.Nombre, Correo = usuario.Correo };
            _dbContext.Usuarios.Add(nuevoUsuario);
            _dbContext.SaveChanges();

            return Ok($"Usuario creado: {usuario.Nombre}");
        }
    }
}