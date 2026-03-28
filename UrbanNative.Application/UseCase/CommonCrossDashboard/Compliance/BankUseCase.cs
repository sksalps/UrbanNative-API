using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCase;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;

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
    public async Task<IEnumerable<BankListDto>> HandleAsync(string entityType,int vendorId)
    {
        return await _bankRepo.GetBankListAsync(entityType, vendorId);
    }

    public async Task ExecuteSetAsync(int bankId, int EntityId,string EntityType)
    {
        await _bankRepo.SetPrimaryBankAsync(bankId, EntityId, EntityType);
    }

    public async Task ExecuteDeleteAsync(int bankId, int EntityId, string EntityType)
    {
        await _bankRepo.DeleteBankAsync(bankId, EntityId, EntityType);
    }
    public async Task<BankSaveRequestDto> GetByIdAsync(int bankId, int EntityId, string EntityType)
    {
        return await _bankRepo.GetByIdAsync(bankId, EntityId, EntityType);
    }

    // ================= SAVE BANK =================
    public async Task SaveBankAsync(IFormFile? file, BankSaveRequestDto dto)
    {
        // 🔹 Get compliance
        var complianceId = await _complianceRepo.GetComplianceByNameAsync(dto.ComplianceName);

        if (complianceId == null)
            throw new Exception("Bank Compliance not configured");

        var compliance = await _complianceRepo.GetComplianceAsync((int)complianceId);
        dto.ComplianceId = compliance.ComplianceID;
        // 🔹 SAVE FILE (if exists)
        if (file != null && file.Length > 0)
        {
            using var stream = file.OpenReadStream();

            // 🔥 Save file physically
            var fileUrl = await _fileStorage.UploadAsync(file,"compliance", dto.ComplianceName,dto.EntityType,dto.EntityID);
            dto.FileURL = fileUrl;
            dto.FileName = file.FileName;
        }
        

        await _bankRepo.SaveBankAsync(dto);
    }

    public async Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file, string complianceName)
    {
        if (string.IsNullOrWhiteSpace(complianceName))
            throw new Exception("Invalid compliance name");

        var complianceId = await _complianceRepo.GetComplianceByNameAsync(complianceName);

        if (complianceId == null)
            throw new Exception("Bank compliance not configured");

        var compliance = await _complianceRepo.GetComplianceAsync((int)complianceId);

        using var stream = file.OpenReadStream();

        var result = await _validationService.ValidateAsync(
            stream,
            file.FileName,
            file.Length,
            compliance
        );

        // 🔥 OCR TEXT SHOULD COME FROM HERE
        var extractedText = result.ExtractedText?.Trim() ?? "";

        // 🔥 PARSE TEXT → BANK FIELDS
        var fields = ExtractChequeText(extractedText);

        result.ExtractedFields = fields;

        return result;
    }
    private Dictionary<string, string> ExtractChequeText(string text)
    {
        var result = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(text))
            return result;

        //text = text.ToUpper();
        //text = Regex.Replace(text, @"\s+", "");

        // 🔹 Account Number (9–18 digits)
        var accMatch = System.Text.RegularExpressions.Regex.Match(text, @"\b\d{9,18}\b");
        if (accMatch.Success)
            result["AccountNo"] = accMatch.Value;

        // 🔹 IFSC
        var ifscMatch = System.Text.RegularExpressions.Regex.Match(text, @"\b[A-Z]{4}0[A-Z0-9]{6}\b");
        if (ifscMatch.Success)
            result["IFSCCode"] = ifscMatch.Value;

        // 🔹 Bank Name (basic heuristic)
        var bankMatch = System.Text.RegularExpressions.Regex.Match(text, @"(STATE BANK|HDFC|ICICI|AXIS|PNB|KOTAK|UNION BANK)");
        if (bankMatch.Success)
            result["BankName"] = bankMatch.Value;

        // 🔹 Account Holder (very basic guess)
        var lines = text.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains("NAME") || line.Contains("A/C"))
            {
                result["AccountHolderName"] = line.Trim();
                break;
            }
        }

        return result;
    }

    
}