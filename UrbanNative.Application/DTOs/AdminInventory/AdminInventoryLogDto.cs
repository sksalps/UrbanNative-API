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

        public string? ValueSignature { get; set; }
        public string ProductName { get; set; } = null!;
        public string VendorName { get; set; } = null!;
    }

    public class AdminInventoryLogPeriodResultDto
    {
        // 🔹 Header (SINGLE object)
        public AdminInventoryLogHeaderDto Header { get; set; }
            = new AdminInventoryLogHeaderDto();

        // 🔹 Logs
        public IEnumerable<AdminInventoryLogDto> Logs { get; set; }
            = Enumerable.Empty<AdminInventoryLogDto>();
    }



    public class AdminInventoryLogHeaderDto
    {
        public string ProductName { get; set; } = "";
        public string VendorName { get; set; } = "";

        public string? ValueSignature { get; set; }

        // 🔹 HUMAN READABLE
        public string VariantDisplay { get; set; } = "";

        public int OpeningStock { get; set; }
        public int ClosingStock { get; set; }
    }

}
