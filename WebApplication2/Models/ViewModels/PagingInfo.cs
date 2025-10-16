namespace WebApplication2.Models.ViewModels
{
    //public class PagingInfo
    //{
    //    public int TotalItems { get; set; }
    //    public int ItemsPerPage { get; set; }
    //    public int CurrentPage { get; set; }
    //    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    //}


    public class PagingInfo
    {
        public int TotalItems { get; set; }      // tổng số sản phẩm
        public int ItemsPerPage { get; set; }    // số sản phẩm trên mỗi trang
        public int CurrentPage { get; set; }     // trang hiện tại

        // Tính tổng số trang dựa trên 2 biến trên
        public int TotalPages
        {
            get
            {
                if (ItemsPerPage == 0) return 0; // tránh chia cho 0
                return (int)System.Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }
    }
}
