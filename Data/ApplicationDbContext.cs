using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CosmeticosApp.Models;

namespace CosmeticosApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurar relaciones
            builder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Producto>()
                .HasOne(p => p.Marca)
                .WithMany(m => m.Productos)
                .HasForeignKey(p => p.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DetallePedido>()
                .HasOne(dp => dp.Pedido)
                .WithMany(p => p.DetallesPedidos)
                .HasForeignKey(dp => dp.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DetallePedido>()
                .HasOne(dp => dp.Producto)
                .WithMany(p => p.DetallesPedidos)
                .HasForeignKey(dp => dp.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar precisión decimal
            builder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            builder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(18, 2);

            builder.Entity<DetallePedido>()
                .Property(dp => dp.PrecioUnitario)
                .HasPrecision(18, 2);

            builder.Entity<DetallePedido>()
                .Property(dp => dp.Subtotal)
                .HasPrecision(18, 2);

            // Semilla de datos
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Categorías
            builder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nombre = "Maquillaje", Descripcion = "Productos de maquillaje para rostro y ojos" },
                new Categoria { Id = 2, Nombre = "Cuidado Facial", Descripcion = "Productos para el cuidado de la piel del rostro" },
                new Categoria { Id = 3, Nombre = "Cuidado Corporal", Descripcion = "Productos para el cuidado del cuerpo" },
                new Categoria { Id = 4, Nombre = "Perfumería", Descripcion = "Fragancias y perfumes" },
                new Categoria { Id = 5, Nombre = "Cuidado del Cabello", Descripcion = "Productos para el cuidado capilar" }
            );

            // Marcas
            builder.Entity<Marca>().HasData(
                new Marca { Id = 1, Nombre = "Maybelline", Descripcion = "Marca líder en maquillaje" },
                new Marca { Id = 2, Nombre = "L'Oréal", Descripcion = "Cosméticos de calidad premium" },
                new Marca { Id = 3, Nombre = "Revlon", Descripcion = "Maquillaje y cuidado personal" },
                new Marca { Id = 4, Nombre = "Neutrogena", Descripcion = "Especialista en cuidado de la piel" },
                new Marca { Id = 5, Nombre = "Dove", Descripcion = "Cuidado corporal y facial" }
            );

            // Productos
            builder.Entity<Producto>().HasData(
                new Producto { Id = 1, Nombre = "Base Líquida FIT ME", Descripcion = "Base de maquillaje líquida con cobertura natural", Precio = 45.99m, Stock = 50, CategoriaId = 1, MarcaId = 1, Codigo = "MAY001", ImagenUrl = "https://images.unsplash.com/photo-1586495777744-4413f21062fa?w=300&h=300&fit=crop" },
                new Producto { Id = 2, Nombre = "Máscara de Pestañas Voluminous", Descripcion = "Máscara que proporciona volumen y longitud", Precio = 32.50m, Stock = 30, CategoriaId = 1, MarcaId = 2, Codigo = "LOR001", ImagenUrl = "https://images.unsplash.com/photo-1512496015851-a90fb38ba796?w=300&h=300&fit=crop" },
                new Producto { Id = 3, Nombre = "Crema Hidratante Facial", Descripcion = "Crema hidratante para todo tipo de piel", Precio = 28.75m, Stock = 40, CategoriaId = 2, MarcaId = 4, Codigo = "NEU001", ImagenUrl = "https://images.unsplash.com/photo-1570194065650-d99fb4bedf0a?w=300&h=300&fit=crop" },
                new Producto { Id = 4, Nombre = "Jabón Corporal Nutritivo", Descripcion = "Jabón corporal con 1/4 de crema hidratante", Precio = 15.99m, Stock = 60, CategoriaId = 3, MarcaId = 5, Codigo = "DOV001", ImagenUrl = "https://images.unsplash.com/photo-1556228453-efd6c1ff04f6?w=300&h=300&fit=crop" },
                new Producto { Id = 5, Nombre = "Labial Mate ColorStay", Descripcion = "Labial de larga duración con acabado mate", Precio = 22.30m, Stock = 35, CategoriaId = 1, MarcaId = 3, Codigo = "REV001", ImagenUrl = "https://images.unsplash.com/photo-1586495777744-4413f21062fa?w=300&h=300&fit=crop&random=2" }
            );
        }
    }
} 