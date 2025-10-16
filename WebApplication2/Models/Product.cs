using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace WebApplication2.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(150)]
        public string? ProductName { get; set; }
        [StringLength(30000)]
        public string? ProductDescription { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal? ProductPrice { get; set; }
        [Column(TypeName = "decimal(2,2)")]
        public decimal? ProductDiscount { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        [StringLength(300)]
        public string? ProductPhoto { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [ForeignKey("Size")]
        public int SizeId { get; set; }
        public virtual Size? Size { get; set; }
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public virtual Color? Color { get; set; }
        public bool IsTrandy { get; set; }
        public bool IsArrived { get; set; }
        public bool IsSportSwear { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsOfficeClothes { get; set; }
        public bool IsWinterClothes { get; set; }
        public bool IsPoloShirt { get; set; }
        public bool IsShorts { get; set; }
    }
}
