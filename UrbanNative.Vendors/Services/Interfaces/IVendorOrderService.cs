using UrbanNative.Application.DTOs.Vendors.Orders;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorOrderService
    {
        Task<IReadOnlyList<VendorOrderListDto>> GetOrdersAsync(
            VendorOrderListFilterDto filter);

        Task<VendorOrderSummaryDto> GetSummaryAsync(
            VendorOrderListFilterDto filter);
        Task<IReadOnlyList<VendorSkuProductSuggestionDto>>        GetSkuProductSuggestionsAsync(string term);
    }

}
