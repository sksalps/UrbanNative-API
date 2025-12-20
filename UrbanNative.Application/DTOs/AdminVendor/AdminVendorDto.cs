namespace UrbanNative.Application.DTOs.AdminVendor
{
    public class AdminVendorListDto
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string? BusinessName { get; set; }

        public string Mobile { get; set; }
        public string? Email { get; set; }

        public int NOP { get; set; }   // Number of Products

        public string? ApprovalStatus { get; set; }
        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    public class AdminVendorDetailDto
    {
        public int VendorID { get; set; }

        public string VendorName { get; set; }
        public string? ContactPerson { get; set; }

        public string Mobile { get; set; }
        public string? Email { get; set; }

        public string? BusinessName { get; set; }
        public string? GSTNumber { get; set; }
        public string? PANNumber { get; set; }

        public string? BusinessAddress { get; set; }
        public string? PickupAddress { get; set; }

        public string? State { get; set; }
        public string? StateCode { get; set; }

        public string? BankAccountNumber { get; set; }
        public string? IFSCCode { get; set; }
        public string? BankName { get; set; }

        public string? ApprovalStatus { get; set; }
        public string? RejectionReason { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AdminVendorApprovalDto
    {
        public int VendorID { get; set; }
        public string ApprovalStatus { get; set; }   // Approved / Rejected
        public string? Reason { get; set; }
    }

    public class AdminVendorActivateDto
    {
        public int VendorID { get; set; }
        public bool IsActive { get; set; }
    }

    public class VendorReferralInfoDto
    {
        public int ReferrerUserID { get; set; }
        public string Name { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public DateTime ReferredAt { get; set; }
    }

    public class VendorMediaDto
    {
        public int VendorMediaID { get; set; }
        public string MediaType { get; set; } = null!; // Photo / Video
        public string MediaUrl { get; set; } = null!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
