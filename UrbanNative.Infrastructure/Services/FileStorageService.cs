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

    public async Task<string> UploadAsync(IFormFile file,string docFor, string folder,string entityType, int entityId )
    {
        //var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", folder);
        var basePath = _config["FileStorage:BasePath"];

        if (!Path.IsPathRooted(basePath))
        {
                basePath = Path.Combine(_env.ContentRootPath, basePath);
        }
        var relativePath = Path.Combine("\\uploads", docFor, entityType, entityId.ToString(), folder);

        var uploadsPath = Path.Combine(basePath,docFor,entityType,entityId.ToString(), folder);
        if (!Directory.Exists(uploadsPath))
        Directory.CreateDirectory(uploadsPath);

        

        //var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}_{file.FileName}" + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        return $"{relativePath}/{fileName}";
        //return $"/uploads/{folder}/{fileName}";
        }
    }
}
