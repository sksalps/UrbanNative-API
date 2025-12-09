using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface IVendorService
    {
        Task<int> RegisterVendorAsync(Vendor vendor);
        Task<Vendor?> LoginVendorAsync(string mobile);
        Task<bool> ApproveVendorAsync(int vendorId, int approvedBy);
        Task<bool> RejectVendorAsync(int vendorId, string reason, int rejectedBy);
        Task<bool> UpdateVendorAsync(Vendor vendor);
        Task<Vendor?> GetVendorByIdAsync(int vendorId);
    }
}