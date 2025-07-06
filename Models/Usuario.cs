using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CosmeticosApp.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Direccion { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relaciones
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
} 