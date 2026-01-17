using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UrbanNative.Application.DTOs.Vendors
{
    public class VendorSystemSettingDto
    {
        public int SystemSettingId { get; set; }
        public string SettingKey { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ValueType { get; set; }

        public decimal CurrentValue { get; set; }

        // 🔥 MUST BE NULLABLE
        public DateTime? EffectiveFrom { get; set; }

        public bool OnlyAdminEditable { get; set; }
    }

    public class VendorSystemSettingUpdateDto
    {
        public int SystemSettingId { get; set; }
        public decimal NewValue { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }

    public class VendorSystemSettingHistoryDto
    {
        public int SystemSettingId { get; set; }
        public decimal Value { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Source { get; set; } // DEFAULT / VENDOR
    }

}
