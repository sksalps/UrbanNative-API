using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Application.UseCase.CommonCroshDashboard.Compliance
{
    public class ComplianceValidateUseCase : IComplianceValidateUseCase
    {
        private readonly IComplianceValidationService _validationService;
        private readonly IComplianceRepository _repository;
        public ComplianceValidateUseCase(
            IComplianceValidationService validationService,IComplianceRepository repository)
        {
            _validationService = validationService;
            _repository = repository;
        }

        public async Task<ComplianceValidationResultDto> ExecuteAsync(Stream fileStream,string fileName,long fileSize,
    int complianceId)
        {
            var compliance = await _repository.GetComplianceAsync(complianceId);
            //Call the validation service to validate the document based on the compliance rules 
            return await _validationService.ValidateAsync(
                fileStream,
                fileName,
                fileSize,
                compliance);
        }
    }
}