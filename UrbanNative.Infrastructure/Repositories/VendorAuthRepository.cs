using Dapper;
using System.ComponentModel.Design;
using System.Data;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;
using UrbanNative.Infrastructure.Security;

namespace UrbanNative.Infrastructure.Repositories
{
    public class VendorAuthRepository : IVendorAuthRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorAuthRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<VendorLoginResultDto?> GetVendorForLoginAsync(string identifier)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<VendorLoginResultDto>(
                "sp_Vendor_Login",
                new { Identifier = identifier },
                commandType: CommandType.StoredProcedure
            );
        }

        //===============Change Password Related Code=================
        public async Task<bool> VerifyPasswordAsync(int vendorId, string password)
        {
            using var conn = _connectionFactory.CreateConnection();

            var data = await conn.QuerySingleOrDefaultAsync<(byte[] Hash, byte[] Salt)>(
                "sp_Vendor_GetPassword",
                new { VendorID = vendorId },
                commandType: CommandType.StoredProcedure);

            if (data.Hash == null || data.Salt == null)
                return false;

            return PasswordHelper.VerifyPassword(password, data.Hash, data.Salt);
        }

        public async Task<bool> UpdatePasswordAsync(int vendorId, string newPassword)
        {
            var (hash, salt) = PasswordHelper.CreateHash(newPassword);

            using var conn = _connectionFactory.CreateConnection();

            int rows = await conn.ExecuteAsync(
                "sp_Vendor_UpdatePassword",
                new
                {
                    VendorID = vendorId,
                    PasswordHash = hash,
                    PasswordSalt = salt
                },
                commandType: CommandType.StoredProcedure);
            if ( rows != 1)
                return false;  
            return true;
        }

        /*
        public async Task<bool> VerifyPasswordAsync(int vendorId, string password)
        {

            //return true;
            using var conn = _connectionFactory.CreateConnection();

            // var result = await conn.QuerySingleOrDefaultAsync<(byte[] Hash, byte[] Salt)>(
            var result = await conn.QuerySingleOrDefaultAsync<(string Hash, string Salt)>(
             "sp_Vendor_GetPassword",
            new { VendorID = vendorId },
            commandType: CommandType.StoredProcedure);

            if (result.Hash == null || result.Salt == null)
            {
                throw new Exception($"Password not found for VendorID {vendorId}");
                return false;
            }
            //return PasswordHelper.VerifyPassword(password, result.Hash, result.Salt);
            return true;
        }

        


       
      public async Task<bool> UpdatePasswordAsync(int vendorId, string newPassword)
      {
          var (hash, salt) = PasswordHelper.CreateHash(newPassword);

          using var conn = _connectionFactory.CreateConnection();

          int rows = await conn.ExecuteAsync(
              "sp_Vendor_UpdatePassword",
              new
              {
                  VendorID = vendorId,
                  PasswordHash = hash,
                  PasswordSalt = salt
              },
              commandType: CommandType.StoredProcedure);

          return rows == 1;
      }



      public async Task<int> ChangePasswordAsync(int vendorId,byte[] oldPasswordHash, byte[] newPasswordHash,
              byte[] newPasswordSalt)
          {
              using var conn = _connectionFactory.CreateConnection();

              var param = new DynamicParameters();
              param.Add("@VendorID", vendorId);
              param.Add("@OldPasswordHash", oldPasswordHash);
              param.Add("@NewPasswordHash", newPasswordHash);
              param.Add("@NewPasswordSalt", newPasswordSalt);
              param.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

              await conn.ExecuteAsync(
                  "sp_Vendor_ChangePassword",
                  param,
                  commandType: CommandType.StoredProcedure);

              return param.Get<int>("@Result");
          }
      */
    }


}
