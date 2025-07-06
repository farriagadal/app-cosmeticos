using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticosApp.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;

        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(200)]
        public string? DireccionEntrega { get; set; }

        [StringLength(500)]
        public string? Notas { get; set; }

        // Relaciones
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual ICollection<DetallePedido> DetallesPedidos { get; set; } = new List<DetallePedido>();
    }
} 