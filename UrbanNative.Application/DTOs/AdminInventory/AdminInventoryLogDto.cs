namespace UrbanNative.Application.DTOs.AdminInventory
{
    public class AdminInventoryLogDto
    {
        public int LogID { get; set; }
        public string ChangeType { get; set; } = null!;
        public int Quantity { get; set; }
        public int OldStock { get; set; }
        public int NewStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public string? VariantSignature { get; set; }
        public string ProductName { get; set; } = null!;
        public string VendorName { get; set; } = null!;
    }

    public class AdminInventoryLogPeriodResultDto
    {
        public int OpeningStock { get; set; }
        public int ClosingStock { get; set; }

        public IEnumerable<AdminInventoryLogDto> Logs { get; set; }
            = Enumerable.Empty<AdminInventoryLogDto>();
        public string ProductName { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string? VariantSignature { get; set; }

    }

}
