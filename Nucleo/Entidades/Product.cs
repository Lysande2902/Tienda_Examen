using System.Collections.Generic;
using System.Linq;

namespace Nucleo.Entidades
{
    public class Product
    {
        public int ID_Producto { get; set; }
        public string Nombre_Producto { get; set; }

        public static void ValidarNombreUnico(
            string nombre,
            IEnumerable<Product> productosExistentes
        )
        {
            if (productosExistentes.Any(p => p.Nombre_Producto == nombre))
                throw new InvalidOperationException("Ya existe un producto con ese nombre.");
        }
    }
}
