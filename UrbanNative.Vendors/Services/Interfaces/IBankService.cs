using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Shared.SharedDTOs;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IBankService
    {
        Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file, string complianceName);
        Task<ServiceResult> SaveFullAsync(IFormFile file, BankSaveRequestDto dto);
    }
}
