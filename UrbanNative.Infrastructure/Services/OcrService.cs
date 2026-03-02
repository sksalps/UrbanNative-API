using Tesseract;

namespace UrbanNative.Infrastructure.Services
{
    public class OcrService
    {
        public string ExtractText(string filePath)
        {
            using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
            using var img = Pix.LoadFromFile(filePath);
            using var page = engine.Process(img);
            return page.GetText();
        }
    }
}