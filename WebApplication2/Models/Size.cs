using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required]
        [StringLength(10)]
        public string? SizeName { get; set; }
        [Required]
        public int SizeOrder { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
