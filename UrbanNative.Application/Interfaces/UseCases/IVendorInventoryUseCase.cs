using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Inventory;

namespace UrbanNative.Application.Interfaces.UseCases
{

        public interface IVendorInventoryUseCase
        {
            Task<VendorInventorySummaryDto> GetInventorySummaryAsync(
                int skuId,
                int addressId,
                DateTime fromDate,
                DateTime toDate);

            Task<IReadOnlyList<VendorInventoryLogDto>> GetInventoryLogsAsync(
                int skuId,
                int addressId,
                DateTime fromDate,
                DateTime toDate,
                int page,
                int pageSize);
        }

    }

