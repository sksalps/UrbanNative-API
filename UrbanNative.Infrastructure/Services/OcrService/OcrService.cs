using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using static System.Net.WebRequestMethods;
//Interface use with OCR.Space API to extract docs from uploaded image

namespace UrbanNative.Infrastructure.Services.OcrService
{

    public class OcrService : IOcrService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OcrService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["OcrSettings:ApiKey"];
        }


        public async Task<string> ExtractTextAsync(Stream fileStream)
        {
            if (fileStream.Length > 5 * 1024 * 1024)
                throw new Exception("File too large for OCR");
            using var image = await Image.LoadAsync(fileStream);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1200, 1200)
            }));
            var compressedStream = new MemoryStream();

            await image.SaveAsync(compressedStream, new JpegEncoder { Quality = 60 });

            compressedStream.Position = 0;

            using var form = new MultipartFormDataContent();

            form.Add(new StreamContent(compressedStream), "file", "doc.jpg");
            form.Add(new StringContent("eng"), "language");
            form.Add(new StringContent("true"), "scale");
            form.Add(new StringContent("2"), "OCREngine");
            form.Add(new StringContent("true"), "isOverlayRequired");

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("apikey", _apiKey);

            var response = await _http.PostAsync("https://api.ocr.space/parse/image", form);

            var json = await response.Content.ReadAsStringAsync();

            var obj = JsonSerializer.Deserialize<OCRResponse>(json);

            return obj?.ParsedResults?.FirstOrDefault()?.ParsedText ?? "";
        }

        public class OCRResponse
        {
            public List<ParsedResult>? ParsedResults { get; set; }
        }

        public class ParsedResult
        {
            public string? ParsedText { get; set; }
        }
    }
}