using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance
{
    //============DTOs used for Menu, Compliance=>Documents List==================//
    public class ComplianceDocumentListDto11
    {
        public int UploadID { get; set; }
        public int ComplianceID { get; set; }
        public string ComplianceName { get; set; }

        public string FileName { get; set; }
        public string FileURL { get; set; }

        public string VerificationStatus { get; set; }
        public string DisplayStatus { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? UploadedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public string RejectionReason { get; set; }
        public string DocumentNumber { get; set; }

        public bool IsCurrent { get; set; }
        public bool IsApprovedVersion { get; set; }

        public string VersionStatus { get; set; }
        public string ApprovedStatus { get; set; }
        public string ExpiryStatus { get; set; }

        public int RowNum { get; set; }
        public int TotalCount { get; set; }

        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
