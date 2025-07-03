using Nucleo.Entidades;
using Nucleo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura
{
    public class RepositorioProducto : IRepositorioProducto
    {
        private readonly TiendaDbContext _context;
        public RepositorioProducto(TiendaDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> ObtenerTodos() => _context.Productos.ToList();
        public Product ObtenerPorId(int id) => _context.Productos.Find(id);
        public Product ObtenerPorNombre(string nombre) => _context.Productos.FirstOrDefault(p => p.Nombre_Producto == nombre);
        public void Agregar(Product producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
        }
        public void Actualizar(Product producto)
        {
            _context.Productos.Update(producto);
            _context.SaveChanges();
        }
        public void Eliminar(int id)
        {
            var prod = _context.Productos.Find(id);
            if (prod != null)
            {
                _context.Productos.Remove(prod);
                _context.SaveChanges();
            }
        }
    }
} 