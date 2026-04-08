using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.CommonCrossDashboard
{
    public class AddressListDto
    {
        public int AddressID { get; set; }
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

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }

        public int CityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }

        public string Pincode { get; set; }
        public bool IsPrimary { get; set; }

        public string AddressNickName { get; set; }

        public string AddressType { get; set; }
    }
}
