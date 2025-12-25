namespace UrbanNative.Application.DTOs.AdminVariant
{
    public class AdminVariantListDto
    {
        public int VariantID { get; set; }
        public string VariantName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalValues { get; set; }
    }

    public class AdminVariantDetailsDto
    {
        public int VariantID { get; set; }
        public string VariantName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class UpdateVariantValueDto
    {
        public string ValueName { get; set; } = null!;
    }

    public class AdminVariantValueDto
    {
        public int VariantValueID { get; set; }
        public int VariantID { get; set; }
        public string ValueName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CreateVariantDto
    {
        public string VariantName { get; set; } = null!;
    }

    public class CreateVariantValueDto
    {
        public int VariantID { get; set; }
        public string ValueName { get; set; } = null!;
    }
}