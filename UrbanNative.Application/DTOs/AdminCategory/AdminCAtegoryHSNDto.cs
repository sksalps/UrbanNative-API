namespace UrbanNative.Application.DTOs.AdminCategory
{
    public class CategoryHSNDto
    {
        public int CategoryId { get; set; }
        public int HSNId { get; set; }

        public string HSNCode { get; set; } = null!;
        public string? Description { get; set; }

        public int GSTId { get; set; }
    }

    public class CategoryHSNLinkDto
    {
        public int CategoryId { get; set; }
        public int HSNId { get; set; }

        public int AdminId { get; set; }
    }
}