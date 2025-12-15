using UrbanNative.Application.DTOs;
public interface IAdminService
{
    Task<AdminInfoDto?> ValidateAdminAsync(string identifier, string password);
    Task<int> GetVendorsCountAsync();
    Task<int> GetPendingProductsCountAsync();
    Task<int> GetLowStockCountAsync();
    Task<int> GetUsersCountAsync();
}
