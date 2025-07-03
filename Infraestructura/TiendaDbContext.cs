using Microsoft.EntityFrameworkCore;
using Nucleo.Entidades;

namespace Infraestructura
{
    public class TiendaDbContext : DbContext
    {
        public TiendaDbContext(DbContextOptions<TiendaDbContext> options) : base(options) { }

        public DbSet<Product> Productos { get; set; }
        public DbSet<Order> Ordenes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Nombre_Producto)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .Property(o => o.Estado)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.ID_Order, oi.ID_Producto });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Producto)
                .WithMany()
                .HasForeignKey(oi => oi.ID_Producto);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Orden)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.ID_Order);
        }
    }
} 