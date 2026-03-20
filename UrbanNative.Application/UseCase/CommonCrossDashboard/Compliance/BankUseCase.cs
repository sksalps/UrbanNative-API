using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using Microsoft.AspNetCore.Http;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCase;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;

public class BankUseCase : IBankUseCase
{
    private readonly IComplianceRepository _complianceRepo; 
    private readonly IComplianceValidationService _validationService;
    private readonly IBankRepository _bankRepo;
    private readonly IFileStorageService _fileStorage;

    public BankUseCase(
        IComplianceRepository complianceRepo,
        IComplianceValidationService validationService,
        IBankRepository bankRepo,
        IFileStorageService fileStorage)
    {
        _complianceRepo = complianceRepo;
        _validationService = validationService;
        _bankRepo = bankRepo;
        _fileStorage = fileStorage;
    }

    // ================= VALIDATE =================
    public async Task<ComplianceValidationResultDto> ValidateAndUploadAsync(
        IFormFile file,
        int uploadId,
        string entityType,
        int entityId)
    {
        // 🔥 STEP 1: Get ComplianceMaster ONCE
        //var compliance = await _complianceRepo.GetByNameAsync("Bank Details");
        var compliance = await _complianceRepo.GetComplianceAsync(9);
        if (compliance == null)
            throw new Exception("Bank compliance not configured");

        using var stream = file.OpenReadStream();

        // 🔥 STEP 2: OCR + Validation
        var validation = await _validationService.ValidateAsync(
            stream,
            file.FileName,
            file.Length,
            compliance
        );

        if (!validation.IsValid)
            return validation;

        // 🔥 STEP 3: Save file physically
        var fileUrl = await _fileStorage.SaveAsync(file, "bank");

        // 🔥 STEP 4: Upsert compliance
        var newUploadId = await _bankRepo.UpsertComplianceAsync(
            uploadId,
            entityType,
            entityId,
            compliance.ComplianceID,
            file.FileName,
            fileUrl,
            validation.ExtractedFields.GetValueOrDefault("AccountNo")
        );

        validation.ExtractedFields["UploadId"] = newUploadId.ToString();

        return validation;
    }


    public async Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file)
    {
        //var compliance = await _complianceRepo.GetByNameAsync("Bank Details");
        var compliance = await _complianceRepo.GetComplianceAsync(9);
        using var stream = file.OpenReadStream();

        var result = await _validationService.ValidateAsync(
            stream,
            file.FileName,
            file.Length,
            compliance
        );

        return result; // 🔥 NO DB CALL
    }
    // ================= UPSERT (NO FILE) =================
    public async Task<int> UpsertAsync(int uploadId, string documentNumber,string entityType,int entityId)
    {
        //var compliance = await _complianceRepo.GetByNameAsync("Bank Details");
        var compliance = await _complianceRepo.GetComplianceAsync(9);

        if (compliance == null)
            throw new Exception("Bank compliance not configured");

        return await _bankRepo.UpsertComplianceAsync(
            uploadId,
            entityType,
            entityId,
            compliance.ComplianceID,
            fileName: null,
            fileUrl: null,
            documentNumber
        );
    }

    // ================= SAVE BANK =================
    public async Task SaveBankAsync(BankSaveRequestDto dto)
    {
        await _bankRepo.SaveBankAsync(dto);
    }
}