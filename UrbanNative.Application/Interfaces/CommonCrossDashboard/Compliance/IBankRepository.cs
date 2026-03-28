using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

namespace UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance
{
        public interface IBankRepository
        {
            Task SaveBankAsync(BankSaveRequestDto dto);
            Task<IEnumerable<BankListDto>> GetBankListAsync(string entityType, int entityId);
        Task SetPrimaryBankAsync(int bankId, int entityId, string entityType);
        Task<BankSaveRequestDto> GetByIdAsync(int bankId, int entityId, string entityType);
        Task DeleteBankAsync(int bankId, int entityId, string entityType);
        }
    
}
