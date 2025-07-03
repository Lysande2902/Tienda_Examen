using Aplicacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Nucleo.Entidades;

namespace API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly ServiciosProducto _servicio;

        public ProductosController(ServiciosProducto servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Los parámetros 'page' y 'pageSize' deben ser positivos.");
            var productos = _servicio.ObtenerTodos().Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id < 0)
                return BadRequest("El id debe ser positivo.");
            var prod = _servicio.ObtenerPorId(id);
            if (prod == null)
                return NotFound();
            return Ok(prod);
        }

        // Métodos POST, PUT y DELETE eliminados para cumplir la regla de negocio: no modificar productos
    }
}
