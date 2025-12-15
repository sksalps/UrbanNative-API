using System.Threading.Tasks;
using UrbanNative.Application.DTOs;
using UrbanNative.Domain.Entities;



namespace UrbanNative.Application.Interfaces
{
    public interface IAdminRepository
    {
        Task<AdminUser?> GetByUsernameOrEmailAsync(string identifier);
    }


    
}




