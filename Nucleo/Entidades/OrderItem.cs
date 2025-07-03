namespace Nucleo.Entidades
{
    public class OrderItem
    {
        public int ID_Producto { get; set; }
        public int ID_Order { get; set; }

        public Product Producto { get; set; }
        public Order Orden { get; set; }
    }
}
