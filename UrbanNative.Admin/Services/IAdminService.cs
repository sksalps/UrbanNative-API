using UrbanNative.Admin.Models;
using System.Threading.Tasks;

namespace UrbanNative.Admin.Services
{
    public interface IAdminService
    {
        Task<AdminInfo?> ValidateAdminAsync(string identifier, string password);

        Task<int> GetVendorsCountAsync();
        Task<int> GetPendingProductsCountAsync();
        Task<int> GetLowStockCountAsync();
        Task<int> GetUsersCountAsync();
    }
}
