//namespace WebApplication2.Models.ViewModels
//{
//    public class ProductListViewModel
//    {
//        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
//        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
//    }
//}
using System.Collections.Generic;

namespace WebApplication2.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
        public int CurrentCategory { get; set; }
    }

    
}
