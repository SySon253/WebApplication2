//namespace WebApplication2.Models
//{
//    public class Cart
//    {
//        public List<CartLine> Lines { get; set; } = new List<CartLine>();
//        public void AddItem(Product product, int quantity)
//        {
//            CartLine? line = Lines
//                .Where(p => p.Product.ProductId == product.ProductId)
//                .FirstOrDefault();
//            if (line == null)
//            {
//                Lines.Add(new CartLine
//                {
//                    Product = product,
//                    Quantity = quantity
//                });
//            }
//            else
//            {
//                line.Quantity += quantity;
//            }
//        }
//        public void RemoveLine(Product product) =>
//            Lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
//        public decimal ComputeTotalValues()
//        {
//            return (decimal)Lines.Sum(e => e.Product?.ProductPrice * (1 - e.Product?.ProductDiscount) * e.Quantity);
//        }
//        public void Clear() => Lines.Clear();
//    }
//    public class CartLine
//    {
//        public int CartLineId { get; set; }
//        public Product Product { get; set; } = new();
//        public int Quantity { get; set; }
//    }
//}



using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WebApplication2.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            var line = Lines.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = (decimal)product.ProductPrice,
                    ProductDiscount = (decimal)product.ProductDiscount,
                    ProductPhoto = product.ProductPhoto,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product) =>
            Lines.RemoveAll(l => l.ProductId == product.ProductId);

        public decimal ComputeTotalValues()
        {
            return Lines.Sum(e => e.ProductPrice * (1 - e.ProductDiscount) * e.Quantity);
        }

        public void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineId { get; set; }

        // ✅ Lưu thông tin cần thiết, tránh lưu toàn bộ Product (vòng lặp object)
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        //public string Size { get; set; }
        //public string Color { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductDiscount { get; set; }
        public string? ProductPhoto { get; set; }

        public int Quantity { get; set; }

        // ❌ Không serialize Product để tránh vòng lặp khi lưu session
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
