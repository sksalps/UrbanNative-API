using UrbanNative.Application.DTOs.AdminReturnsOrder;

namespace UrbanNative.Admin.Services
{
    public interface IAdminReturnsService
    {
        Task<IEnumerable<AdminReturnListDto>> GetReturnsAsync(string? status,      int? vendorId,
            DateTime? fromDate,
            DateTime? toDate);

        Task<AdminReturnDetailsDto?> GetReturnDetailsAsync(int returnId);
        Task<List<ReturnImageDto>> GetImagesAsync(int returnId);
        // Task<bool> ApproveRejectAsync(       int returnId,            bool isApproved,     string adminComment,            string remarkText);
        Task<IEnumerable<LogisticsProviderDto>> GetLogisticsProvidersAsync();
        Task<bool> CreateReturnShipmentAsync(int returnId, int logisticsProviderID, string trackingNumber);
        Task<bool> UpdateStatusAsync( int returnId,  string newStatus, string adminComment,    string remarkText);
    }

}
