using UrbanNative.Application.DTOs.AdminReturnsOrder;

namespace UrbanNative.Admin.Services
{
    public interface IAdminReturnsService
    {
        Task<IEnumerable<AdminReturnListDto>> GetReturnsAsync(string? status,      int? vendorId,
            DateTime? fromDate,
            DateTime? toDate);

        Task<AdminReturnDetailsDto?> GetReturnDetailsAsync(int returnId);

        Task<bool> ApproveRejectAsync(
            int returnId,
            bool isApproved,
            string adminComment,
            string remarkText);

        Task<bool> CreateReturnShipmentAsync(
            int returnId,
            string courierName,
            string trackingNumber,
            string pickupAddress,
            string deliveryAddress);
    }

}
