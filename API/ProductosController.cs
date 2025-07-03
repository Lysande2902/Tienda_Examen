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
            var productos = _servicio.ObtenerTodos().Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var prod = _servicio.ObtenerPorId(id);
            if (prod == null)
                return NotFound();
            return Ok(prod);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product producto)
        {
            try
            {
                _servicio.Agregar(producto);
                return CreatedAtAction(nameof(Get), new { id = producto.ID_Producto }, producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product producto)
        {
            if (id != producto.ID_Producto)
                return BadRequest();
            try
            {
                _servicio.Actualizar(producto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _servicio.Eliminar(id);
            return NoContent();
        }
    }
}
