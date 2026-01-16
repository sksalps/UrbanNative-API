using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Application.UseCases.Vendors
{
    public class VendorProfileUseCase : IVendorProfileUseCase
    {
        private readonly IVendorProfileRepository _repository;

        public VendorProfileUseCase(IVendorProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<VendorProfileDto> GetProfileAsync(int vendorId)
        {
            // Simple read — no business rule yet
            return await _repository.GetProfileAsync(vendorId);
        }

        public async Task UpdateProfileAsync(int vendorId, VendorProfileDto profile)
        {
            // 🔒 Business rules (Phase-1 minimal)
            if (string.IsNullOrWhiteSpace(profile.BusinessName))
                throw new ArgumentException("Business name is required");

            if (string.IsNullOrWhiteSpace(profile.Email))
                throw new ArgumentException("Email is required");

            await _repository.UpdateProfileAsync(vendorId, profile);
        }
    }
}
