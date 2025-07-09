using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CosmeticosApp.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; } // Precio en CLP (pesos chilenos)

        [Required]
        public int Stock { get; set; }

        public string? ImagenUrl { get; set; }

        [StringLength(50)]
        public string? Codigo { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Claves foráneas
        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public int MarcaId { get; set; }

        // Relaciones
        [ValidateNever]
        public virtual Categoria? Categoria { get; set; }

        [ValidateNever]
        public virtual Marca? Marca { get; set; }

        [ValidateNever]
        public virtual ICollection<DetallePedido> DetallesPedidos { get; set; } = new List<DetallePedido>();
    }
} 