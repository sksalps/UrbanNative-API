using UrbanNative.Infrastructure.Repositories;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repo;

        public VendorService(IVendorRepository repo)
        {
            _repo = repo;
        }

        public Task<int> RegisterVendorAsync(Vendor vendor)
            => _repo.RegisterVendorAsync(vendor);

        public Task<Vendor?> LoginVendorAsync(string mobile)
            => _repo.LoginVendorAsync(mobile);

        public Task<bool> ApproveVendorAsync(int vendorId, int approvedBy)
            => _repo.ApproveVendorAsync(vendorId, approvedBy);

        public Task<bool> RejectVendorAsync(int vendorId, string reason, int rejectedBy)
            => _repo.RejectVendorAsync(vendorId, reason, rejectedBy);

        public Task<bool> UpdateVendorAsync(Vendor vendor)
            => _repo.UpdateVendorAsync(vendor);

        public Task<Vendor?> GetVendorByIdAsync(int vendorId)
            => _repo.GetVendorByIdAsync(vendorId);
    }
}