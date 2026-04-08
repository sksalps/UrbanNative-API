using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard;
using UrbanNative.Application.UseCase.CommonCroshDashboard.Compliance;

namespace UrbanNative.Api.Controllers.CommonCrossDasboard
{
    [Authorize]
    [ApiController]
    [Route("api/complianceocr")]
    public class OcrComplianceController : Controller
    {
        private readonly IOcrComplianceUseCase _useCase;
        public OcrComplianceController(IOcrComplianceUseCase useCase)
        {
            _useCase = useCase;
        }
        [HttpPost("validate-extract")]
        public async Task<IActionResult> ValidateExtractCompliance([FromForm] IFormFile File, [FromForm] int ComplianceId, [FromForm] string Regex)
        {
            //Console.WriteLine("API HIT");
            try
            {
                if (File == null || File.Length == 0)
                    return BadRequest("File missing");

                var result = await _useCase.OcrFileValidateAsync(
                    File.OpenReadStream(),
                    File.FileName,
                    File.Length,
                    ComplianceId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }
    }
}
