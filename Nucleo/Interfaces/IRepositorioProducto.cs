namespace Nucleo.Interfaces
{
    using Nucleo.Entidades;
    public interface IRepositorioProducto
    {
        IEnumerable<Product> ObtenerTodos();
        Product ObtenerPorId(int id);
        Product ObtenerPorNombre(string nombre);
        void Agregar(Product producto);
        void Actualizar(Product producto);
        void Eliminar(int id);
    }
} 