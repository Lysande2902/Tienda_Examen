namespace Nucleo.Interfaces
{
    using Nucleo.Entidades;
    public interface IRepositorioOrden
    {
        IEnumerable<Order> ObtenerTodos();
        Order ObtenerPorId(int id);
        void Agregar(Order orden);
        void Actualizar(Order orden);
        void Eliminar(int id);
    }
} 