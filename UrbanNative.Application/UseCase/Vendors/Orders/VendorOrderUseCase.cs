using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Orders;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.Vendors.Orders;


namespace UrbanNative.Application.UseCase.Vendors.Orders
{
    public class VendorOrderUseCase : IVendorOrderUseCase
    {
        private readonly IVendorOrderRepository _repository;

        public VendorOrderUseCase(IVendorOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<VendorOrderListDto>> GetOrdersAsync(
            VendorOrderListFilterDto filter)
        {
            return await _repository.GetVendorOrdersAsync(filter);
        }

        public async Task<VendorOrderSummaryDto> GetSummaryAsync(
            VendorOrderListFilterDto filter)
        {
            return await _repository.GetVendorOrderSummaryAsync(filter);
        }
        public async Task<IReadOnlyList<VendorSkuProductSuggestionDto>>
            GetSkuProductSuggestionsAsync(int vendorId, string term)
        {
            return await _repository.GetSkuProductSuggestionsAsync(vendorId, term);
        }

        // Vendor Order Details View

        public async Task<VendorOrderDetailsDto> ExecuteAsync(
        int vendorId,
        int? orderId,
        int? shipmentId,
        int? returnId)
        {
            return await _repository.GetVendorOrderDetailsAsync(
                vendorId, orderId, shipmentId, returnId);
        }

    }

}
