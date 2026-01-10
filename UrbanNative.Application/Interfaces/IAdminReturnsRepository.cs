using UrbanNative.Application.DTOs.AdminReturnsOrder;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminReturnsRepository
    {
        Task<IEnumerable<AdminReturnListDto>> GetReturnsAsync(
            string? status,
            int? vendorId,
            DateTime? fromDate,
            DateTime? toDate);
        Task<bool> UpdateStatusAsync(   int returnId,    int adminId,    string newStatus,    string adminComment,    string remarkText);
        Task<IEnumerable<ReturnImageDto>> GetReturnImagesAsync(int returnId);
        Task<bool> AddReturnImageAsync(int returnId, string imageUrl, string uploadedBy, bool isPrimary);

        Task<AdminReturnDetailsDto?> GetReturnDetailsAsync(int returnId);
        Task<IEnumerable<LogisticsProviderDto>> GetLogisticsProvidersAsync();

        Task<bool> ApproveRejectAsync(
            int returnId,
            int adminId,
            bool isApproved,
            string adminComment,
            string remarkText);

        Task<bool> CreateReturnShipmentAsync(
            int returnId, int logisticsProviderID,
            string trackingNumber);

    }
}
