namespace UrbanNative.Application.DTOs.AdminHSN
{
    public class AdminHSNListDto
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; } = null!;
        public string? Description { get; set; }

        public int GSTId { get; set; }
        public decimal GSTPercentage { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AdminHSNCreateDto
    {
        public string HSNCode { get; set; } = null!;
        public string? Description { get; set; }

        public int GSTId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class AdminHSNUpdateDto
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; } = null!;
        public string? Description { get; set; }

        public int GSTId { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class AdminHSNDetailDto
    {
        public int HSNId { get; set; }
        public string HSNCode { get; set; } = null!;
        public string? Description { get; set; }

        public int GSTId { get; set; }
        public bool IsActive { get; set; }
    }
}