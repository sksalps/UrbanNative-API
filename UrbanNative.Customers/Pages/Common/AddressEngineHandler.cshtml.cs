using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.CommonCrossDashboard; // change for Admin

namespace UrbanNative.Customers.Pages.Common
{
    public class AddressEngineHandlerModel : PageModel
    {
        private readonly IAddressEngineService _addressService;

        public AddressEngineHandlerModel(IAddressEngineService addressService)
        {
            _addressService = addressService;
        }

        /* =========================
           🔹 COUNTRY
        ========================= */
        public async Task<IActionResult> OnGetCountries()
        {
            try
            {
                var data = await _addressService.GetCountriesAsync(null);

                if (data == null)
                    return new JsonResult(new List<object>()); // ✅ never null

                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /* =========================
           🔹 STATE
        ========================= */
        public async Task<IActionResult> OnGetStates(int countryId)
        {
            var data = await _addressService.GetStatesAsync(countryId, null);
            return new JsonResult(data);
        }

        /* =========================
           🔹 CITY
        ========================= */
        public async Task<IActionResult> OnGetCities(int stateId)
        {
            var data = await _addressService.GetCitiesAsync(stateId, null);
            return new JsonResult(data);
        }

        // ============================================================
        // 🔷 ADDRESS HANDLERS (MODAL)
        // ============================================================

        // 🔹 Get Address List
        public async Task<JsonResult> OnGetAddressLookupAsync(string type, int? entityId = null)
        {
            var data = await _addressService.GetAddressesLookupAsync(type);
            return new JsonResult(data);
        }

        // 🔹 Get Address By ID (Edit Mode)
        public async Task<JsonResult> OnGetAddressByIdAsync(int addressId)
        {
            var data = await _addressService.GetByIdAsync(addressId);
            return new JsonResult(data);
        }


        /* =========================
           🔹 SAVE ADDRESS
        ========================= */
        public async Task<IActionResult> OnPostSaveAddress([FromBody] AddressSaveDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResult
                {
                    IsSuccess = false,
                    Message = "Validation failed"
                });
            }

            var result = await _addressService.SaveAddressAsync(dto);

            return new JsonResult(new
            { 
                success = result.IsSuccess,
                message = result.Message,
                addressId = result.AddressId
            });
        }


        /* =========================
         * AddressList Page
         ===========================*/

        public async Task<IActionResult> OnGetAddressList()
        {
            var data = await _addressService.GetAddressListAsync();
            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostDeleteAddress(int id)
        {

            try
            {
                var result = await _addressService.DeleteAddressAsync(id);

                return new JsonResult(new
                {
                    success = result.IsSuccess,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }




        public async Task<IActionResult> OnPostSetPrimaryAsync(int id)
        {
            try
            {
                var result = await _addressService.SetPrimaryAddressAsync(id);

                return new JsonResult(new
                {
                    success = result.IsSuccess,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}