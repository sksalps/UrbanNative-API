namespace UrbanNative.Application.DTOs
{
    public class AdminNotificationDto
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Title { get; set; } = "";
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
