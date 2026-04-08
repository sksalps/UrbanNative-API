using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Application.Interfaces.CommonCrossDashboard
{
    public interface IOcrComplianceInfraService
    {
        //Service method to validate the extracted data from
        //the document based on the compliance rules,
        //Call 3rd party OCR API to extract text and then validate against regex or keywords
        Task<ComplianceValidationResultDto> ValidateExtractAsync(
            Stream fileStream,
            string fileName,
            long fileSize,
            ComplianceMaster compliance);
    }
    
}
