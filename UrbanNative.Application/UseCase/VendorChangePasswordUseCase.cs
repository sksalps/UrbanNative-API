using UrbanNative.Application.DTOs.Common;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Application.UseCases.Vendors  
{

    public class VendorChangePasswordUseCase : IVendorChangePasswordUseCase
    {
        private readonly IVendorAuthRepository _repository;

        public VendorChangePasswordUseCase(  IVendorAuthRepository repository)
        {
            _repository = repository;
        }

        public async Task<ChangePasswordResult> ExecuteAsync(
        int vendorId,
        string oldPassword,
        string newPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword) ||
                string.IsNullOrWhiteSpace(newPassword))
                return ChangePasswordResult.InvalidInput;

            if (oldPassword == newPassword)
                return ChangePasswordResult.SamePassword;

            var isValid = await _repository.VerifyPasswordAsync(vendorId, oldPassword);
            //isValid=true; // Temporarily bypassing old password verification for testing purposes.
            if (!isValid)
                return ChangePasswordResult.InvalidOldPassword;

            var result=  await _repository.UpdatePasswordAsync(  vendorId,      newPassword);

            if (!result)
                return ChangePasswordResult.Failed;
            return ChangePasswordResult.Success;
        }
    }

}

