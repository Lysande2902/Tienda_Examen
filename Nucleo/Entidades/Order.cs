namespace Nucleo.Entidades
{
    public class Order
    {
        public int ID_Order { get; set; }
        public string Estado { get; set; } // Pendiente, Pagado, Enviado
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public bool PuedeModificarItems()
        {
            return Estado != "Enviado";
        }

        public void CambiarEstado(string nuevoEstado)
        {
            if (Estado == "Enviado")
                throw new InvalidOperationException("No se puede cambiar el estado de un pedido enviado.");
            Estado = nuevoEstado;
        }

        public void AgregarItem(OrderItem item)
        {
            if (!PuedeModificarItems())
                throw new InvalidOperationException("No se pueden modificar los items de un pedido enviado.");
            Items.Add(item);
        }

        public void EliminarItem(OrderItem item)
        {
            if (!PuedeModificarItems())
                throw new InvalidOperationException("No se pueden modificar los items de un pedido enviado.");
            Items.Remove(item);
        }
    }
} 