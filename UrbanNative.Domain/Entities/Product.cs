namespace UrbanNative.Domain.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal MRP { get; set; }
        public decimal DiscountPrice { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int VendorID { get; set; }
        public bool IsPriceInclusive { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? IGST { get; set; }
        public string? ApprovalStatus { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? RejectedReason { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}