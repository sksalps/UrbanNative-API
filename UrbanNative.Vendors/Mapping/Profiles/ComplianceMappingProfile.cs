using AutoMapper;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Shared.Models.Compliance;

namespace UrbanNative.Vendors.Mapping.Profiles
{
    public class ComplianceMappingProfile : Profile
    {
        public ComplianceMappingProfile()
        {
            CreateMap<ComplianceDocumentListDto, ComplianceDocumentViewModel>();
        }
    }
}