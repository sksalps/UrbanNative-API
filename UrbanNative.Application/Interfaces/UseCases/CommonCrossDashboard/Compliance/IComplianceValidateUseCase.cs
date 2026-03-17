using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

namespace UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance
{
    public interface IComplianceValidateUseCase
    {
        Task<ComplianceValidationResultDto> ExecuteAsync(
            Stream fileStream,
            string fileName,
            long fileSize,
            int complianceId);
    }
}
