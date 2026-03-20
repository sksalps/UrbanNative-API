using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

namespace UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance
{

    public interface IBankUseCase
    {
        Task<ComplianceValidationResultDto> ValidateAndUploadAsync(
            IFormFile file,
            int uploadId,
            string entityType,
            int entityId);

        Task<int> UpsertAsync(int uploadId,string documentNumber,string entityType,      int entityId);
        Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file);
        Task SaveBankAsync(BankSaveRequestDto dto);
    }
}
