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

        Task<AdminReturnDetailsDto?> GetReturnDetailsAsync(int returnId);

        Task<bool> ApproveRejectAsync(
            int returnId,
            int adminId,
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
