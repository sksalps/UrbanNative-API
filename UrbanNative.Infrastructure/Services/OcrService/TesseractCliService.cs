using System.Diagnostics;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;

public class TesseractCliService 
{
    private readonly string _tesseractPath = "tesseract"; // or full path if needed

    public async Task<string> ExtractTextAsync(Stream fileStream)
    {
        // 🔥 Save temp file
        var tempFile = Path.GetTempFileName() + ".jpg";

        using (var fs = new FileStream(tempFile, FileMode.Create))
        {
            await fileStream.CopyToAsync(fs);
        }

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = _tesseractPath,
                Arguments = $"{tempFile} stdout -l eng",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);

            string result = await process.StandardOutput.ReadToEndAsync();

            await process.WaitForExitAsync();

            return result;
        }
        catch
        {
            return "";
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }
}