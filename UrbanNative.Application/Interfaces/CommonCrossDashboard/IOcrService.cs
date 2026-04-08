using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Interface for OCR service to extract docs from uploaded document
// Exist in Infrastructure layer, but defined here to avoid circular dependency with Application layer
// Implementation name is OcrService.cs, but can be implemented with any OCR provider (e.g. Tesseract, Azure OCR, Google Vision)
namespace UrbanNative.Application.Interfaces.CommonCrossDashboard
{
    public interface IOcrService
    {
        Task<string> ExtractTextAsync(Stream fileStream);
    }
}
