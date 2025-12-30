using UrbanNative.Application.DTOs.AdminGST;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminGSTRepository
    {
        Task<IEnumerable<AdminGSTListDto>> GetGSTAsync( decimal? gstPercentage,      bool? isActive);

        //Task ToggleGSTAsync(    int gstId,   bool isActive,         int adminId);
        Task<bool> ToggleGSTAsync(int gstId, int adminId);
    }
}