using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

namespace UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard
{
    public interface IOcrComplianceUseCase
    {
        Task<ComplianceValidationResultDto> OcrFileValidateAsync(Stream fileStream,string fileName,long fileSize,int complianceId);
    }
}
