using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Vendors.Products
{
        public class VendorProductListDto
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public string CategoryName { get; set; }
            public bool HasVariants { get; set; }
            public string ApprovalStatus { get; set; }
            public bool IsActive { get; set; }
            public string SkuStatus { get; set; }   // computed
            public DateTime CreatedAt { get; set; }
        }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
