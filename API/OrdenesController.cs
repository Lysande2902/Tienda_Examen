using System.Linq;
using Aplicacion;
using Microsoft.AspNetCore.Mvc;
using Nucleo.Entidades;

namespace API
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesController : ControllerBase
    {
        private readonly ServiciosOrden _servicio;

        public OrdenesController(ServiciosOrden servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 10)
        {
            var ordenes = _servicio.ObtenerTodos().Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var orden = _servicio.ObtenerPorId(id);
            if (orden == null)
                return NotFound();
            return Ok(orden);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order orden)
        {
            _servicio.Agregar(orden);
            return CreatedAtAction(nameof(Get), new { id = orden.ID_Order }, orden);
        }

        [HttpPut("{id}/estado")]
        public IActionResult CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            try
            {
                _servicio.CambiarEstado(id, nuevoEstado);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/items")]
        public IActionResult AgregarItem(int id, [FromBody] OrderItem item)
        {
            try
            {
                _servicio.AgregarItem(id, item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/items/{idProducto}")]
        public IActionResult EliminarItem(int id, int idProducto)
        {
            try
            {
                _servicio.EliminarItem(id, idProducto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
