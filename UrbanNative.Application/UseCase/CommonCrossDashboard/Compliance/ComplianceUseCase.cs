using System.IO;
using System.Text.RegularExpressions;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;

namespace UrbanNative.Application.UseCase.CommonCroshDashboard.Compliance
{
    public class ComplianceUseCase : IComplianceUseCase
    {
        private readonly IComplianceRepository _repository;
        private readonly IOcrService _ocrService;

        public ComplianceUseCase(IComplianceRepository repository, IOcrService ocrService)
        {
            _repository = repository;
            _ocrService = ocrService;
        }

        // 🔝 Dashboard Summary
        public async Task<IEnumerable<ComplianceScopeDashboardSummaryDto>> GetDashboardSummaryAsync(
            string entityType,
            int entityId)
        {
            var result = await _repository.GetDashboardSummaryAsync(entityType, entityId);

            // Future: attach global messages, approval flags, etc.
            return result;
        }
        // 📊 Category Progress Strip
        public async Task<IEnumerable<ComplianceCategoryProgressDto>> GetCategoryProgressAsync(
            string entityType,
            int entityId)
        {
            return await _repository.GetCategoryProgressAsync(entityType, entityId);
        }
        // 📋 Category Grid
        public async Task<IEnumerable<ComplianceCategoryGridDto>> GetCategoryGridAsync(
            string entityType,
            int entityId)
        {
            return await _repository.GetCategoryGridAsync(entityType, entityId);
        }

        // 📄 Documents inside Category (Grid-1)
        public async Task<IEnumerable<ComplianceDocumentDto>> GetDocumentsByCategoryAsync(
            string entityType,
            int entityId,
            int groupId)
        {
            return await _repository.GetDocumentsByCategoryAsync(entityType, entityId, groupId);
        }

        // 📚 Document History (Grid-2)
        public async Task<IEnumerable<ComplianceDocumentHistoryDto>> GetDocumentHistoryAsync(
            string entityType,
            int entityId,
            int complianceId)
        {
            return await _repository.GetDocumentHistoryAsync(entityType, entityId, complianceId);
        }

        // 🟢 Category Status Banner
        public async Task<ComplianceCategoryStatusDto> GetCategoryStatusAsync(
            string entityType,
            int entityId,
            int groupId)
        {
            return await _repository.GetCategoryStatusAsync(entityType, entityId, groupId);
        }
        public async Task UploadDocumentAsync(
        string entityType,
        int entityId,
        ComplianceUploadRequest request,
        Stream fileStream,
        string fileName)
        {
            await _repository.UploadDocumentAsync(entityType, entityId, request, fileStream, fileName);
        }
        
        public async Task<string?> ExtractDocumentNumberAsync(Stream fileStream, string regex)
        {
            if (string.IsNullOrWhiteSpace(regex))
                return null;
            fileStream.Position = 0;
            //var text = await _ocrService.ExtractTextAsync(fileStream);
            var text = "";
            text = text.ToUpper();
            text = Regex.Replace(text, @"\s+", "");

            var match = Regex.Match(text, regex, RegexOptions.IgnoreCase);

            return match.Success ? match.Value : null;
        }
    }
}
