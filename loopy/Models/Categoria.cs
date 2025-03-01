using System.ComponentModel.DataAnnotations;

namespace loopy.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}
    