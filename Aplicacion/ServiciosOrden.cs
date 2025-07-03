using Nucleo.Entidades;
using Nucleo.Interfaces;

namespace Aplicacion
{
    public class ServiciosOrden
    {
        private readonly IRepositorioOrden _repoOrden;
        private readonly IRepositorioOrderItem _repoItem;
        public ServiciosOrden(IRepositorioOrden repoOrden, IRepositorioOrderItem repoItem)
        {
            _repoOrden = repoOrden;
            _repoItem = repoItem;
        }

        public IEnumerable<Order> ObtenerTodos() => _repoOrden.ObtenerTodos();
        public Order ObtenerPorId(int id) => _repoOrden.ObtenerPorId(id);
        public void Agregar(Order orden) => _repoOrden.Agregar(orden);
        public void CambiarEstado(int id, string nuevoEstado)
        {
            var orden = _repoOrden.ObtenerPorId(id);
            orden.CambiarEstado(nuevoEstado);
            _repoOrden.Actualizar(orden);
        }
        public void AgregarItem(int idOrden, OrderItem item)
        {
            var orden = _repoOrden.ObtenerPorId(idOrden);
            orden.AgregarItem(item);
            _repoItem.Agregar(item);
        }
        public void EliminarItem(int idOrden, int idProducto)
        {
            var orden = _repoOrden.ObtenerPorId(idOrden);
            var item = _repoItem.ObtenerPorOrden(idOrden).FirstOrDefault(i => i.ID_Producto == idProducto);
            if (item != null)
            {
                orden.EliminarItem(item);
                _repoItem.Eliminar(idOrden, idProducto);
            }
        }
    }
} 