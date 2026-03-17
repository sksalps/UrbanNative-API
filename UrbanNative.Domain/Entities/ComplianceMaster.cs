namespace UrbanNative.Domain.Entities;

public class ComplianceMaster
{
    public int ComplianceID { get; set; }

    public string ComplianceName { get; set; } = "";

    public string Category { get; set; } = "";

    public string DocumentType { get; set; } = "";

    public string? AllowedFileTypes { get; set; }

    public int? MaxFileSizeMB { get; set; }

    public int? DefaultExpiryMonths { get; set; }

    public int? ExpiryAlertDays { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public string ComplianceMode { get; set; } = "";

    public string? MsgCode { get; set; }

    public bool HasNumberField { get; set; }

    public string? NumberFieldLabel { get; set; }

    public string? NumberFieldRegex { get; set; }

    public bool HasExpiry { get; set; }

    public string? OCRKeywords { get; set; }
}