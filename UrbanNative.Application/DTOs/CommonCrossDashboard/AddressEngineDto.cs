using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.CommonCrossDashboard
{
    public class AddressListDto
    {
        public int AddressID { get; set; }
        public string AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }

        public int CityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }

        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }

        public string Pincode { get; set; }
        public bool IsPrimary { get; set; }

        public string AddressNickName { get; set; } = "";
    }

    
    public class AddressSaveDto
    {
        public int? AddressID { get; set; }
        public int? EntityID { get; set; }
        public string? EntityType { get; set; } = null;


        [Required]
        [MinLength(7)]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Landmark { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        public string Pincode { get; set; }

        public bool IsPrimary { get; set; }

        [Required]
        public string AddressType { get; set; }

        [Required]
        public string AddressNickName { get; set; }
    }


    public class CountryDto
    {
        public int CountryID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
    public class StateDto
    {
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string GSTCode { get; set; }
    }
    public class CityDto
    {
        public int CityID { get; set; }
        public int StateID { get; set; }
        public string CityName { get; set; }
    }
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? AddressId { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }

}
