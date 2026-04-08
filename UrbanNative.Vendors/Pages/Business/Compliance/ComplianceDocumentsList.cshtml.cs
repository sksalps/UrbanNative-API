using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard;
using UrbanNative.Shared.Models.Compliance;
using UrbanNative.Vendors.Services;

public class ComplianceDocumentsModel : PageModel
{
    private readonly IVendorComplianceService _service;
    private readonly IConfiguration _configuration;
    private readonly IOcrComplianceInfraService _infraservice;
    public string ApiBaseUrl { get; private set; } = "";

    public ComplianceDocumentsModel(IVendorComplianceService service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
        ApiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? ""; 
    }

    public List<ComplianceDocumentViewModel> Documents   { get; set; }
   

    public async Task OnGet()
    {
        Documents = await _service.GetUploadedDocumentsListAsync();
    }

    // ✅ DELETE HANDLER
    public async Task<IActionResult> OnPostDeleteAsync(int uploadId)
    {
        try
        {
            await _service.DeleteDocumentAsync(uploadId);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

    public async Task<IActionResult> OnPostUploadDocumentAsync()
    {
        try
        {
            var form = Request.Form;

            //var uploadId = Convert.ToInt32(form["UploadID"]);
            var uploadId = int.TryParse(form["UploadID"], out var id) ? id : 0;
            var complianceId = Convert.ToInt32(form["ComplianceID"]);
            
            DateTime? expiryDate = null;

            if (DateTime.TryParse(form["ExpiryDate"], out var parsedDate))
            {
                expiryDate = parsedDate;
            }
            
            var documentNumber = form["DocumentNumber"];
            

            var file = Request.Form.Files.FirstOrDefault();

            await _service.UploadDocumentAsync( complianceId,uploadId, file, expiryDate, documentNumber);

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

    public async Task<IActionResult> OnPostOcrDocumentAsync(IFormFile File, int complianceId, string regex)
    {
        if (File == null || File.Length == 0)
        {
            return new JsonResult(new { isValid = false, message = "Invalid file" });
        }
        var value = await _service.ExtractDocumentAsync(File, regex, complianceId);
        return new JsonResult(new { value });
    }
}