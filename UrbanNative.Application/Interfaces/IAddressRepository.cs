using UrbanNative.Application.DTOs.Address;

namespace UrbanNative.Application.Interfaces
{
    public interface IAddressRepository
    {
        Task<int> SaveAddressAsync(AddressCreateDto dto);
        Task<IEnumerable<AddressDto>> GetAddressesAsync(string entityType, int entityId);
        Task DeactivateAddressAsync(int addressId);
    }
}