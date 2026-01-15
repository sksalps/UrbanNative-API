using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors
{
    public class VendorDashboardDto
    {
        public int MyProducts { get; set; }
        public int PendingProducts { get; set; }
        public int LowStockSkus { get; set; }
        public int ActiveOrders { get; set; }
    }

}
