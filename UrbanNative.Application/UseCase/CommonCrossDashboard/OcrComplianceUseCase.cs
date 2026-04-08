using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard;

namespace UrbanNative.Application.UseCase.CommonCrossDashboard
{
    public class OcrComplianceUseCase : IOcrComplianceUseCase
    {
        private readonly IOcrComplianceInfraService _validationService;
        private readonly IComplianceRepository _repository;
        public OcrComplianceUseCase(IOcrComplianceInfraService validationService, IComplianceRepository repository)
        {
            _validationService = validationService;
            _repository = repository;
        }

        public async Task<ComplianceValidationResultDto> OcrFileValidateAsync(Stream fileStream, string fileName, long fileSize,
    int complianceId)
        {
            var compliance = await _repository.GetComplianceAsync(complianceId);
            //Call the validation service to validate the document based on the compliance rules 
            return await _validationService.ValidateExtractAsync(fileStream,fileName,fileSize,compliance);
        }
    }
}
