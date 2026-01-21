using UrbanNative.Application.DTOs.Common;

namespace UrbanNative.Application.Interfaces.UseCases

{
    public interface IVendorChangePasswordUseCase
    {
        Task<ChangePasswordResult> ExecuteAsync(          int vendorId,
            string oldPassword,
            string newPassword);
    }

}
