namespace UrbanNative.Admin.Models
{
    public class OrderFilterModel
    {
        public string? OrderNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? PaymentStatus { get; set; }
        public string? OrderStatus { get; set; }

        public int? UserID { get; set; }
        public int? VendorID { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
