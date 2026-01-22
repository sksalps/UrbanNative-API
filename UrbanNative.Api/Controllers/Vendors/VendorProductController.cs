using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [ApiController]
    [Route("api/vendors/products")]
    public class VendorProductsController : ControllerBase
    {
        private readonly IVendorProductsUseCase _useCase;

        public VendorProductsController(IVendorProductsUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? q,
            int? categoryId,
            int? hsnId)
        {
            int vendorId = GetVendorId()/* from auth */;

            var result = await _useCase.ExecuteAsync(
                vendorId,
                q,
                categoryId,
                hsnId
            );

            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> Categories()
        {
            int vendorId = GetVendorId()/* from auth */;
            return Ok(await _useCase.GetVendorCategoriesAsync(vendorId));
        }

        [HttpGet("hsn")]
        public async Task<IActionResult> Hsn()
        {
            return Ok(await _useCase.GetVendorHsnListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendorProductCreateDto dto)
        {
            int vendorId = GetVendorId()/* from auth */;

            var id = await _useCase.ExecuteAsync(vendorId, dto);
            return Ok(id);
        }
        /*
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int vendorId = GetVendorId();
            var data = await _useCase.ExecuteAsync(vendorId);
            return Ok(data);
        }
        */
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            int vendorId = GetVendorId();

            var data = await _useCase.ExecuteAsync(vendorId, id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] VendorProductUpdateDto dto)
        {
            int vendorId = GetVendorId();

            dto.ProductID = id;

            await _useCase.ExecuteAsync(vendorId, dto);

            return Ok();
        }
        private int GetVendorId()
        {
            var vendorIdClaim = User.FindFirst("VendorId")?.Value;
            //vendorIdClaim = "1";
            if (string.IsNullOrWhiteSpace(vendorIdClaim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return int.Parse(vendorIdClaim);
        }

    }



}
