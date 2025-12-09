using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IVendorRepository
    {
        Task<int> RegisterVendorAsync(Vendor vendor);
        Task<Vendor?> LoginVendorAsync(string mobile);
        Task<bool> ApproveVendorAsync(int vendorId, int approvedBy);
        Task<bool> RejectVendorAsync(int vendorId, string reason, int rejectedBy);
        Task<bool> UpdateVendorAsync(Vendor vendor);
        Task<Vendor?> GetVendorByIdAsync(int vendorId);
    }
}