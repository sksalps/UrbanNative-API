using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance
{
    public class ComplianceDashboardSummaryDto
    {
        public string ApprovalStatus { get; set; } = string.Empty;   // APPROVED / PENDING
        public DateTime? ApprovedOn { get; set; }
        public DateTime? PayoutEligibleOn { get; set; }
        public decimal OverallProgressPercent { get; set; }
    }
    public class ComplianceScopeDashboardSummaryDto
    {
        public string ScopeName { get; set; }
        public string Status { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public decimal ProgressPercent { get; set; }
        public decimal OverallProgressPercent { get; set; }
    }
    public class ComplianceCategoryProgressDto
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;

        public int MinRequired { get; set; }
        public int TotalDocs { get; set; }
        public int ApprovedDocs { get; set; }
        public bool IsComplete { get; set; } = true;
        public int MissingMandatoryDocs { get; set; }   // PAN missing scenario

        public decimal ProgressPercent =>
            MinRequired == 0 ? 0 : Math.Min(100, (ApprovedDocs * 100m) / MinRequired);
    }
    public class ComplianceCategoryGridDto
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;

        public int TotalDocs { get; set; }
        public int UploadedDocs { get; set; }
        public int ApprovedDocs { get; set; }

        public DateTime? LastUpdated { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Scopes { get; set; } = string.Empty;
    }
    public class ComplianceDocumentDto
    {
        public int ComplianceID { get; set; }
        public string ComplianceName { get; set; } = string.Empty;
        
        public string ComplianceCategory { get; set; } = string.Empty;

        //public string ImportanceLevel { get; set; } = string.Empty;   // Mandatory/Required/Optional
        public string ComplianceMode { get; set; } = string.Empty;    // UPLOAD / UPDATE / SYSTEM

        public string? VerificationStatus { get; set; }   // APPROVED / PENDING / REJECTED / NULL
        public DateTime? ExpiryDate { get; set; }
        public DateTime? UploadedAt { get; set; }
        public bool HasExpiry { get; set; }
        public int? DefaultExpiryMonths { get; set; }
        public int? ExpiryAlertDays { get; set; }
        public string ActionType { get; set; } = string.Empty;   // Upload / Update / View / Re-upload
        public int? MaxFileSizeMB { get; set; }
        public string AllowedFileTypes { get; set; } = string.Empty;
        public bool IsMandatoryInGroup { get; set; }   // PAN must              // 
        public bool HasNumberField { get; set; }
        public string? NumberFieldLabel { get; set; }
        public string? NumberFieldRegex { get; set; }
        public string? FileURL { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public bool IsCategoryMandatory { get; set; }
        public bool IsDocMandatory { get; set; }
        public int CategoryMandatoryDocCount { get; set; }
    }
    public class ComplianceValidationResultDto
    {
        public bool IsValid { get; set; }

        public string? DetectedNumber { get; set; }

        public string Message { get; set; } = "";
        // 🔥 ADD THIS for Bank details extraction and
        // other future use cases where we might want to return multiple extracted fields from the document
        public Dictionary<string, string> ExtractedFields { get; set; } = new();
        public string? ExtractedText { get; set; } = "";
    }
    
    public class ComplianceValidateRequestDto
    {
        public Stream FileStream { get; set; } = default!;
        public string FileName { get; set; } = "";
        public long FileSize { get; set; }
        public int ComplianceId { get; set; }
        public string? Regex { get; set; }
    }
    
    public class ComplianceDocumentHistoryDto
    {
        public int UploadID { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string VerificationStatus { get; set; } = string.Empty;
        public string ApprovedStatus { get; set; } = string.Empty;

        public DateTime? ExpiryDate { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public string? RejectionReason { get; set; }
        public string? FileURL { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string VersionStatus { get; set; } = string.Empty;   // Current / Old
        public bool IsApprovedVersion { get; set; }
    }

    public class ComplianceCategoryStatusDto
    {
        public string GroupName { get; set; } = string.Empty;

        public int MinRequired { get; set; }
        public int ApprovedDocs { get; set; }
        public int UploadedDocs { get; set; }
        public int MissingMandatoryDocs { get; set; }

        public bool IsComplete => MissingMandatoryDocs == 0 && ApprovedDocs >= MinRequired;
    }

    public class ComplianceUploadRequest
    {
        public int ComplianceID { get; set; }
        public int UploadID { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? DocumentNumber { get; set; }   // NEW

    }

    public class OcrResponseDto
    {
        public string? Value { get; set; }
    }


    //============DTOs used for Menu, Compliance=>Documents List==================//
        public class ComplianceDocumentListDto
        {
        public int UploadID { get; set; } =0;
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
