using Nucleo.Entidades;
using Nucleo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura
{
    public class RepositorioOrden : IRepositorioOrden
    {
        private readonly TiendaDbContext _context;
        public RepositorioOrden(TiendaDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Order> ObtenerTodos() => _context.Ordenes.Include(o => o.Items).ToList();
        public Order ObtenerPorId(int id) => _context.Ordenes.Include(o => o.Items).FirstOrDefault(o => o.ID_Order == id);
        public void Agregar(Order orden)
        {
            _context.Ordenes.Add(orden);
            _context.SaveChanges();
        }
        public void Actualizar(Order orden)
        {
            _context.Ordenes.Update(orden);
            _context.SaveChanges();
        }
        public void Eliminar(int id)
        {
            var orden = _context.Ordenes.Find(id);
            if (orden != null)
            {
                _context.Ordenes.Remove(orden);
                _context.SaveChanges();
            }
        }
    }
} 