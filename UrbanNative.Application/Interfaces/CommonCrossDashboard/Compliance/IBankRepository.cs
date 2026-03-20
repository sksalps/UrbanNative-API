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
            Task<int> UpsertComplianceAsync(
                int uploadId,
                string entityType,
                int entityId,
                int complianceId,
                string? fileName,
                string? fileUrl,
                string? documentNumber
            );

            Task SaveBankAsync(BankSaveRequestDto dto);
        }
    
}
