namespace UrbanNative.Application.DTOs.AdminGST
{

    public class AdminGSTListDto
    {
        public int GSTID { get; set; }
        public string GSTCode { get; set; }
        public string GSTName { get; set; }
        public decimal GSTPercentage { get; set; }
        public string GSTType { get; set; }
        public bool IsExempt { get; set; }
        public string Source { get; set; }
        public string? GovtGSTCode { get; set; }
        public bool IsActive { get; set; }
        // 🔹 NEW (STEP-2)
        public int HSNCount { get; set; }
    }

   

}