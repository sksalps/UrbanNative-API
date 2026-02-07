using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Orders;

namespace UrbanNative.Application.Interfaces.UseCases
{
    public interface IVendorOrderUseCase
    {
        Task<IReadOnlyList<VendorOrderListDto>> GetOrdersAsync(            VendorOrderListFilterDto filter);

        Task<VendorOrderSummaryDto> GetSummaryAsync(            VendorOrderListFilterDto filter);
        Task<IReadOnlyList<VendorSkuProductSuggestionDto>>    GetSkuProductSuggestionsAsync(int vendorId, string term);

    }

}
