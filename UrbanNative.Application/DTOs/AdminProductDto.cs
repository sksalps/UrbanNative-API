namespace UrbanNative.Application.DTOs
{
    public class AdminProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public decimal MRP { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Stock { get; set; }
        public string ApprovalStatus { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
