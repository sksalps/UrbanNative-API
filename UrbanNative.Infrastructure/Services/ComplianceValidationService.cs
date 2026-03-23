using System.Text.RegularExpressions;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;
namespace UrbanNative.Infrastructure.Services
{
    public class ComplianceValidationService : IComplianceValidationService
    {
        private readonly IOcrService _ocrService;

        public ComplianceValidationService(IOcrService ocrService)
        {
            _ocrService = ocrService;
        }

        public async Task<ComplianceValidationResultDto> ValidateAsync(
            Stream fileStream,
            string fileName,
            long fileSize,
            ComplianceMaster compliance)
        {
            // File type validation
            if (!ValidateFileType(fileName, compliance.AllowedFileTypes))
            {
                return new ComplianceValidationResultDto
                {
                    IsValid = false,
                    Message = "Invalid file type"
                };
            }

            // File size validation
            if (!ValidateFileSize(fileSize, compliance.MaxFileSizeMB ?? 5))
            {
                return new ComplianceValidationResultDto
                {
                    IsValid = false,
                    Message = "File size exceeds allowed limit"
                };
            }

            // OCR
            fileStream.Position = 0;
            
            var text = await _ocrService.ExtractTextAsync(fileStream);

            text = text.ToUpper();
            text = Regex.Replace(text, @"\s+", " ");

            // Keyword validation
            if (!ValidateOCRKeywords(text, compliance.OCRKeywords))
            {
                return new ComplianceValidationResultDto
                {
                    IsValid = false,
                    Message = $"Uploaded document is blur or does not appear to be {compliance.ComplianceName}"
                };
            }

            // Number validation
            if (compliance.HasNumberField && !string.IsNullOrWhiteSpace(compliance.NumberFieldRegex))
            {
                var number = ExtractNumber(text, compliance.NumberFieldRegex);

                if (number == null)
                {
                    return new ComplianceValidationResultDto
                    {
                        IsValid = false,
                        Message = $"{compliance.NumberFieldLabel} not detected"
                    };
                }

                return new ComplianceValidationResultDto
                {
                    IsValid = true,
                    DetectedNumber = number,
                    //ExtractedText = text,
                    Message = $"{compliance.NumberFieldLabel} detected"
                };
            }

            return new ComplianceValidationResultDto
            {
                IsValid = true,
                ExtractedText = text,
                Message = "Document validated successfully"
            };
        }

        private bool ValidateFileType(string fileName, string allowedTypes)
        {
            if (string.IsNullOrWhiteSpace(allowedTypes))
                return true;

            var allowed = allowedTypes.Split(',')
                .Select(x => x.Trim().ToLower());

            var ext = Path.GetExtension(fileName)
                .Replace(".", "")
                .ToLower();

            return allowed.Contains(ext);
        }

        private bool ValidateFileSize(long fileSize, int maxSizeMB)
        {
            var maxBytes = maxSizeMB * 1024 * 1024;

            return fileSize <= maxBytes;
        }

        private bool ValidateOCRKeywords(string text, string? keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
                return true;

            var list = keywords.Split(',');

            foreach (var word in list)
            {
                if (text.Contains(word.Trim(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private string? ExtractNumber(string text, string regex)
        {
            var match = Regex.Match(text, regex, RegexOptions.IgnoreCase);

            return match.Success ? match.Value : null;
        }
        
    }
}