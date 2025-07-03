using Nucleo.Entidades;
using Nucleo.Interfaces;

namespace Aplicacion
{
    public class ServiciosProducto
    {
        private readonly IRepositorioProducto _repo;
        public ServiciosProducto(IRepositorioProducto repo)
        {
            _repo = repo;
        }

        public IEnumerable<Product> ObtenerTodos() => _repo.ObtenerTodos();
        public Product ObtenerPorId(int id) => _repo.ObtenerPorId(id);
        public void Agregar(Product producto)
        {
            Product.ValidarNombreUnico(producto.Nombre_Producto, _repo.ObtenerTodos());
            _repo.Agregar(producto);
        }
        public void Actualizar(Product producto)
        {
            Product.ValidarNombreUnico(producto.Nombre_Producto, _repo.ObtenerTodos().Where(p => p.ID_Producto != producto.ID_Producto));
            _repo.Actualizar(producto);
        }
        public void Eliminar(int id) => _repo.Eliminar(id);
    }
} 