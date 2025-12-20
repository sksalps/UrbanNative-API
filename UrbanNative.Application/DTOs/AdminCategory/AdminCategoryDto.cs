namespace UrbanNative.Application.DTOs.AdminCategory
{
    // =========================
    // Admin – Category List DTO
    // =========================
    public class AdminCategoryListDto
    {
        public int CategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Level { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }

        // UI helpers
        public int ChildCount { get; set; }
    }

    // =========================
    // Admin – Category Detail DTO
    // =========================
    public class AdminCategoryDetailDto
    {
        public int CategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Level { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    // =========================
    // Admin – Create / Update DTO
    // =========================
    public class   AdminCategorySaveDto
    {
        public int? ParentCategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
    }

    public class ToggleResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

}
