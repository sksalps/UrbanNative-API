using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
namespace  UrbanNative.Infrastructure.Services
{

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveAsync(IFormFile file, string folder)
    {
        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", folder);

        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative URL
        return $"/uploads/{folder}/{fileName}";
    }
}
}
