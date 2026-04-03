namespace UrbanNative.Shared.Models.Compliance
{
    public class ComplianceDocumentViewModel
    {
        public int UploadID { get; set; } = 0;
        public int ComplianceID { get; set; }
        public string ComplianceName { get; set; }

        public string FileName { get; set; }
        public string FileURL { get; set; }
        public string VerificationStatus { get; set; }
        public string DisplayStatus { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? UploadedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public string RejectionReason { get; set; }
        public string DocumentNumber { get; set; }

        public bool IsCurrent { get; set; }
        public bool IsApprovedVersion { get; set; }

        public string VersionStatus { get; set; }
        public string ApprovedStatus { get; set; }
        public string ExpiryStatus { get; set; }

        public int RowNum { get; set; }
        public int TotalCount { get; set; }

        public bool HasNumberField { get; set; }
        public string NumberFieldLabel { get; set; }
        public string NumberFieldRegex { get; set; }
        public string AllowedFileTypes { get; set; }
        public int? MaxFileSizeMB { get; set; }
        public bool HasExpiry { get; set; }
        public int? DefaultExpiryMonths { get; set; }

        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanUpload { get; set; }

    }
}