using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors
{
  
        public class VendorProfileDto
        {
            public string BusinessName { get; set; }
            public string ContactPerson { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
        }

    public class VendorSettingsDto
    {
        public bool EmailNotifications { get; set; }
        public bool OrderAlerts { get; set; }
    }



}
