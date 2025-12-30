using UrbanNative.Application.DTOs.AdminHSN;

namespace UrbanNative.Admin.Services
{
    public interface IAdminHSNService
    {
        Task<IEnumerable<AdminHSNListDto>> GetAllAsync();
        Task<AdminHSNDetailDto?> GetByIdAsync(int hsnId);

        Task CreateAsync(AdminHSNCreateDto dto);
        Task UpdateAsync(int hsnId, AdminHSNUpdateDto dto);

        Task ToggleActiveAsync(int hsnId);
    }
}