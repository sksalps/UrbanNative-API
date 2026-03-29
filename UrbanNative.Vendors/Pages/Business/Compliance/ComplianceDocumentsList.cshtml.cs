using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Shared.Models.Compliance;
using UrbanNative.Vendors.Services;

public class ComplianceDocumentsModel : PageModel
{
    private readonly IVendorComplianceService _service;

    public ComplianceDocumentsModel(IVendorComplianceService service)
    {
        _service = service;
    }

    public List<ComplianceDocumentViewModel>
    Documents
    { get; set; }

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

            var uploadId = Convert.ToInt32(form["UploadID"]);
            var complianceId = Convert.ToInt32(form["ComplianceID"]);
            DateTime? expiryDate = null;

            if (DateTime.TryParse(form["ExpiryDate"], out var parsedDate))
            {
                expiryDate = parsedDate;
            }
            
            var documentNumber = form["DocumentNumber"];
            

            var file = Request.Form.Files.FirstOrDefault();

            await _service.UploadDocumentAsync( complianceId, file, expiryDate, documentNumber);

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}