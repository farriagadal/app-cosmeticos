using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticosApp.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; } // Precio unitario en CLP (pesos chilenos)

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; } // Subtotal en CLP (pesos chilenos)

        // Relaciones
        public virtual Pedido Pedido { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
} 