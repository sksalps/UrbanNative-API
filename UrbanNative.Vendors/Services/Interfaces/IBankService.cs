using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IBankService
    {
        Task<int> UpsertComplianceAsync(int uploadId,         string accountNo       );

        //Task<int> UploadAsync(     IFormFile file,            int complianceId,   string entityType,            int entityId);
        Task SaveAsync(BankSaveRequestDto dto);
    }
}
