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
        

        //Task<int> UpsertAsync(IFormFile file, string[] allowed, int uploadId, string documentNumber, string entityType, int entityId);
        Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file, string complianceName);
        Task SaveBankAsync(IFormFile file, BankSaveRequestDto dto);
    }
}
