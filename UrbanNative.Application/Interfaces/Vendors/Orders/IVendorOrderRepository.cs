using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Orders;

namespace UrbanNative.Application.Interfaces.Vendors.Orders
{
    public interface IVendorOrderRepository
    {
        Task<IReadOnlyList<VendorOrderListDto>> GetVendorOrdersAsync(
            VendorOrderListFilterDto filter);

        Task<VendorOrderSummaryDto> GetVendorOrderSummaryAsync(
            VendorOrderListFilterDto filter);
        Task<IReadOnlyList<VendorSkuProductSuggestionDto>>    GetSkuProductSuggestionsAsync(int vendorId, string term);

        // Vendor Order Details View
        Task<VendorOrderDetailsDto> GetVendorOrderDetailsAsync(
        int vendorId,
        int? orderId,
        int? shipmentId,
        int? returnId    );
    }
}
