using UrbanNative.Application.DTOs.AdminHSN;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminHSNRepository
    {
        Task<int> CreateAsync(AdminHSNCreateDto dto);
        Task UpdateAsync(AdminHSNUpdateDto dto);

        Task<IEnumerable<AdminHSNListDto>> GetFilterAsync(string? search,int? gstId,bool? isActive);
        Task<AdminHSNDetailDto?> GetByIdAsync(int hsnId);

        Task ToggleActiveAsync(int hsnId, int updatedBy);
    }
}