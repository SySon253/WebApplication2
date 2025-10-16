using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required]
        [StringLength(30)]
        public string? ColorName { get; set; }
        [Required]
        public int ColorOrder { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
