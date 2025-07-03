using System.Linq;
using Aplicacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Nucleo.Entidades;

namespace API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            if (page < 1 || pageSize < 1)
                return BadRequest("Los parámetros 'page' y 'pageSize' deben ser positivos.");
            var ordenes = _servicio.ObtenerTodos().Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(ordenes);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id < 0)
                return BadRequest("El id debe ser positivo.");
            var orden = _servicio.ObtenerPorId(id);
            if (orden == null)
                return NotFound();
            return Ok(orden);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order orden)
        {
            if (orden.ID_Order < 0)
                return BadRequest("El id debe ser positivo.");
            _servicio.Agregar(orden);
            return CreatedAtAction(nameof(Get), new { id = orden.ID_Order }, orden);
        }

        [HttpPut("{id}/state")]
        public IActionResult CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            if (id < 0)
                return BadRequest("El id debe ser positivo.");
            if (string.IsNullOrWhiteSpace(nuevoEstado))
                return BadRequest("El estado no puede estar vacío.");
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
        public IActionResult AgregarItem(int id, [FromBody] AgregarItemRequest request)
        {
            if (id < 0)
                return BadRequest("El id debe ser positivo.");
            if (request == null || request.ID_Producto < 0)
                return BadRequest("El ID_Producto es obligatorio y debe ser positivo.");
            var orden = _servicio.ObtenerPorId(id);
            if (orden == null)
                return NotFound("La orden no existe.");
            if (orden.Items.Any(i => i.Producto != null && i.Producto.ID_Producto == request.ID_Producto))
                return BadRequest("El producto ya está en la orden.");
            try
            {
                var item = new OrderItem { ID_Producto = request.ID_Producto };
                _servicio.AgregarItem(id, item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class AgregarItemRequest
        {
            public int ID_Producto { get; set; }
        }

        // Eliminar ítems de una orden está prohibido por la regla de negocio
    }
}
