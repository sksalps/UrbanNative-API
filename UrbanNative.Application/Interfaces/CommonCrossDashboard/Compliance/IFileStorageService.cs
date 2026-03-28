using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file, string docFor, string folder, string entityType, int entityId);
    }
}
