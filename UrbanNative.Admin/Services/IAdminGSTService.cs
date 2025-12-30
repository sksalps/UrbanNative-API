using UrbanNative.Application.DTOs.AdminGST;


namespace UrbanNative.Admin.Services
{
    public interface IAdminGSTService
    {
        Task<IEnumerable<AdminGSTListDto>> GetGSTAsync(decimal? gstPercentage,bool? isActive);
        Task ToggleGSTAsync(int gstId, bool isActive);
    }
}
