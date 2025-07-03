namespace Nucleo.Interfaces
{
    using Nucleo.Entidades;
    public interface IRepositorioOrderItem
    {
        IEnumerable<OrderItem> ObtenerPorOrden(int idOrden);
        void Agregar(OrderItem item);
        void Eliminar(int idOrden, int idProducto);
    }
} 