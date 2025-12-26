namespace UrbanNative.Application.DTOs.AdminVariantSet
{
    //✅ Variant Set – List DTO
    public class AdminVariantSetListDto
    {
        public int VariantSetID { get; set; }
        public string VariantSetName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalVariants { get; set; }
    }
    public class MoveVariantRequestDto
    {
        public string Direction { get; set; } = ""; // "UP" or "DOWN"
    }


    public class AddVariantToSetRequestDto
    {
        public int VariantID { get; set; }
    }

    public class AssignVariantSetCategoryRequestDto
    {
        public int CategoryID { get; set; }
    }

    public class UpdateVariantOrderRequestDto
    {
        public int SortOrder { get; set; }
    }


    //✅ Variant Set – Details DTO
    public class AdminVariantSetDetailsDto
    {
        public int VariantSetID { get; set; }
        public string VariantSetName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    //✅ Variant Inside Variant Set DTO
    public class AdminVariantInsideSetDto
    {
        public int VariantSetVariantID { get; set; }
        public int VariantID { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    //✅ Assigned Category DTO
    public class AdminVariantSetCategoryDto
    {
        public int CategoryVariantSetID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }

    //✅ Create / Update DTOs
    public class CreateVariantSetDto
    {
        public string VariantSetName { get; set; } = string.Empty;
    }

    public class UpdateVariantSetDto
    {
        public string VariantSetName { get; set; } = string.Empty;
    }
}

