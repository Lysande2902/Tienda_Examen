using System.Collections.Generic;
using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nucleo.Entidades
{
    [Table("orders")]
    public class Order
    {
        [Key]
        public int ID_Order { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Pagado, Enviado
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