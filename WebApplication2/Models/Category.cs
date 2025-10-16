using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(150)]
        public string? CategoryName { get; set; }
        [Required]
        [StringLength(300)]
        public string? CategoryPhoto { get; set; }
        [Required]
        public int CategoryOrder { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
