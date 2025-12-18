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
        public string RejectedReason { get; set; } =string.Empty;
        public List<ProductImageDto> Images { get; set; } = new();
        public bool IsActive { get; set; }

    }
    public class ApproveProductRequest
    {
        public string? Remark { get; set; }
    }
    // ===============================
    // DTO: Reject product
    // ===============================
    public class RejectProductRequest
    {
        public string Reason { get; set; } = string.Empty;
    }

}
