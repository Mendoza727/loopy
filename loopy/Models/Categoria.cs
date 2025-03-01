using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using loopy.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(100)]
    public string Nombre { get; set; }

    [JsonIgnore] // Evita problemas de referencia circular y serialización
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
