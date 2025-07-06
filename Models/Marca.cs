using System.ComponentModel.DataAnnotations;

namespace CosmeticosApp.Models
{
    public class Marca
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public string? LogoUrl { get; set; }

        public bool Activa { get; set; } = true;

        // Relaciones
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
} 