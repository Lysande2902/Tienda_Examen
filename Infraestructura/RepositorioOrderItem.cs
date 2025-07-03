using Nucleo.Entidades;
using Nucleo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura
{
    public class RepositorioOrderItem : IRepositorioOrderItem
    {
        private readonly TiendaDbContext _context;
        public RepositorioOrderItem(TiendaDbContext context)
        {
            _context = context;
        }
        public IEnumerable<OrderItem> ObtenerPorOrden(int idOrden) => _context.OrderItems.Where(i => i.ID_Order == idOrden).ToList();
        public void Agregar(OrderItem item)
        {
            _context.OrderItems.Add(item);
            _context.SaveChanges();
        }
        public void Eliminar(int idOrden, int idProducto)
        {
            var item = _context.OrderItems.FirstOrDefault(i => i.ID_Order == idOrden && i.ID_Producto == idProducto);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                _context.SaveChanges();
            }
        }
    }
} 