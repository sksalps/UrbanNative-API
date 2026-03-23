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
        }
    
}
