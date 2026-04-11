using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors
{
    public class VendorWarehouseListDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string ContactPerson { get; set; }
        public string Mobile { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }

        public int AddressID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }

        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
    public class VendorWarehouseSaveDto
    {
        public int? WarehouseId { get; set; }

        public string WarehouseName { get; set; }
        public string ContactPerson { get; set; }
        public string Mobile { get; set; }

        public int AddressID { get; set; }

        public bool IsPrimary { get; set; }
        //public int EntityId { get; set; }
        //public string EntityType { get; set; }
    }
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
