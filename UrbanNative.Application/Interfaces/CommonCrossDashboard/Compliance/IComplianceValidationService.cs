using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance
{
    public interface IComplianceValidationService
    {
        Task<ComplianceValidationResultDto> ValidateAsync(
            Stream fileStream,
            string fileName,
            long fileSize,
            ComplianceMaster compliance);
    }
}
