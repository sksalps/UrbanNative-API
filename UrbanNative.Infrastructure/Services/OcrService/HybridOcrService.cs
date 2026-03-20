using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Infrastructure.Services.OcrService;

public class HybridOcrService
{
    //private readonly OcrSpaceService _ocrSpace;
    private readonly TesseractCliService _tesseract;

    public HybridOcrService(
        //OcrSpaceService ocrSpace,
        TesseractCliService tesseract)
    {
        //_ocrSpace = ocrSpace;
        _tesseract = tesseract;
    }

    public async Task<string> ExtractTextAsync(Stream fileStream)
    {
        try
        {
            fileStream.Position = 0;

            var text = "";//await _ocrSpace.ExtractTextAsync(fileStream);

            if (!string.IsNullOrWhiteSpace(text) && text.Length > 20)
                return text;

            // fallback trigger
        }
        catch
        {
            // OCR.Space failed → fallback
        }

        // 🔥 FALLBACK
        fileStream.Position = 0;
        return await _tesseract.ExtractTextAsync(fileStream);
    }
}