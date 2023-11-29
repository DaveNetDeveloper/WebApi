using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QRsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<QRsController> _logger;

        public QRsController(ILogger<QRsController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("ObtenerQR/{id}")]
        public IActionResult ObtenerQRById(Guid id)
        {
            var qrDb = _dbContext.QRs.Find(id);
            return qrDb != null ? Ok(qrDb) : NoContent();
        }

        [HttpGet("ObtenerQRByCode")]
        public IActionResult ObtenerQRByCode([FromQuery] string qrCode)
        {
            var qrnDb = _dbContext.QRs
            .Where(r => r.qrCode.ToLower() == qrCode.ToLower())
            .ToList();

            return qrnDb != null ? Ok(qrnDb) : NoContent();
        }

        [HttpGet("ObtenerQRs")]
        public IActionResult ObtenerQRs()
        {
            var qrsDbList = _dbContext.QRs.ToList();
            return (qrsDbList != null && qrsDbList.Count > 0) ? Ok(qrsDbList) : NoContent();
        }

        [HttpPost("CrearQR")]
        public IActionResult CrearQR([FromBody] QR qr)
        {
            var nuevoQR = new QR {
                id = new Guid(),
                idProducto = qr.idProducto,
                activo = qr.activo,
                multicliente = qr.multicliente,
                qrCode = qr.qrCode,
                consumido = qr.consumido,
                fechaExpiracion = qr.fechaExpiracion
            };
            _dbContext.QRs.Add(nuevoQR);
            _dbContext.SaveChanges(); 

            return Ok(nuevoQR);
        }

        [HttpPut("ActualizarQR")]
        public IActionResult ActualizarQR([FromBody] QR qr)
        {
            var qrDb = _dbContext.QRs
           .Where(r => r.id == qr.id)
           .SingleOrDefault();

            if (qrDb != null) {
                qrDb.idProducto = qr.idProducto;
                qrDb.activo = qr.activo;
                qrDb.multicliente = qr.multicliente;
                qrDb.qrCode = qr.qrCode;
                qrDb.consumido = qr.consumido;
                qrDb.fechaExpiracion = qr.fechaExpiracion;

                _dbContext.SaveChanges();
                return Ok(qrDb);
            }
            else {
                return NotFound();
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(Guid id)
        {
            var qrDb = _dbContext.QRs
           .Where(r => r.id == id)
           .SingleOrDefault();

            if (qrDb != null) {
                _dbContext.QRs.Remove(qrDb);
                _dbContext.SaveChanges();
                return Ok($"El QR [{qrDb.qrCode}] se ha eliminado correctamente.");
            }
            else {
                return NotFound();
            }
        }
    }
}