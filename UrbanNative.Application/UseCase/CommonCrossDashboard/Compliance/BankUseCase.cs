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
    // ================= SAVE BANK =================
    public async Task SaveBankAsync(IFormFile file, BankSaveRequestDto dto)
    {
        var complianceId = await _complianceRepo.GetComplianceByNameAsync(dto.ComplianceName);
        if (complianceId == null)
        {
            throw new Exception("Bank compliance not configured");
        }
        var compliance = await _complianceRepo.GetComplianceAsync((int)complianceId); //temp hardcoded, should be by name
        if (compliance == null)
        {
            throw new Exception("Bank compliance not configured");
        }
        using var stream = file.OpenReadStream();

        // 🔥 Save file physically
        var fileUrl = await _fileStorage.SaveAsync(file, "bank");
        dto.FileURL = fileUrl;
        dto.FileName = file.FileName;
        dto.ComplianceId = compliance.ComplianceID;
        await _bankRepo.SaveBankAsync(dto);
    }


    public async Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file, string complianceName)
    {
        complianceName = complianceName ?? "";
        
        if (complianceName == "")
            throw new Exception("Invalid compliance name");

        var complianceId = await _complianceRepo.GetComplianceByNameAsync(complianceName);
        if (complianceId == null)
        {
            throw new Exception("Bank compliance not configured");
        }
        var compliance = await _complianceRepo.GetComplianceAsync((int)complianceId); //temp hardcoded, should be by name
        if (compliance == null) { 
            throw new Exception("Bank compliance not configured");
        }
        using var stream = file.OpenReadStream();
        var result = await _validationService.ValidateAsync(
            stream,
            file.FileName,
            file.Length,
            compliance
        );

        return result; // 🔥 NO DB CALL
    }

    
}