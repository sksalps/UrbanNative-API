using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
namespace  UrbanNative.Infrastructure.Services
{

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;

    public FileStorageService(IWebHostEnvironment env, IConfiguration config)
    {
        _env = env;
        _config = config;
    }


        public async Task<string> UploadAsync(
    IFormFile file,
    string docFor,
    string folder,
    string entityType,
    int entityId)
        {
            var basePath = _config["FileStorage:BasePath"];
            var uploadFolder = _config["FileStorage:UploadFolder"];

            if (!Path.IsPathRooted(basePath))
            {
                basePath = Path.Combine(_env.ContentRootPath, basePath);
            }

            // 🔥 Build relative path (OS path)
            string relativePath = folder != null
                ? Path.Combine(uploadFolder, docFor, entityType, entityId.ToString(), folder)
                : Path.Combine(uploadFolder, docFor, entityType, entityId.ToString());

            var uploadsPath = Path.Combine(basePath, relativePath);

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // 🔥 Clean filename (important)
            var originalName = Path.GetFileName(file.FileName);
            var fileName = $"{Guid.NewGuid()}_{originalName}";

            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 🔥 Convert to URL-safe path
            var urlPath = Path.Combine(relativePath, fileName)
                                .Replace("\\", "/");

            // 🔥 Ensure leading slash
            if (!urlPath.StartsWith("/"))
                urlPath = "/" + urlPath;

            return urlPath;
        }
    }
}
