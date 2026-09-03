using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Customers
{

        public class CustomersDashboardDto
        {
            public WelcomeDto Welcome { get; set; }
            public ShipmentSnapshotDto Shipment { get; set; }
            public List<RecentOrderDto> RecentOrders { get; set; }
            public ReferralDto Referral { get; set; }
            public List<ShipmentActivityDto> ShipmentActivities { get; set; }
        }
        public class WelcomeDto
        {
            public int UserID { get; set; }
            public string FullName { get; set; }
            public string UserNickName { get; set; }
            public string ReferralCode { get; set; }
        }

        public class ShipmentSnapshotDto
        {
            public int PendingCount { get; set; }
            public int ShippedCount { get; set; }
            public int DeliveredCount { get; set; }
            public int FailedCount { get; set; }
        }

        public class RecentOrderDto
        {
            public int OrderID { get; set; }
            public string OrderNo { get; set; }
            public string ProductName { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal PayableAmount { get; set; }
            public string OrderStatus { get; set; }
        }

        public class ReferralDto
        {
            public string ReferralCode { get; set; }
            public string ReferralLink { get; set; }
        }

        public class ShipmentActivityDto
        {
            public int ShipmentID { get; set; }
            public int OrderID { get; set; }
            public string ProductName { get; set; }
            public string ShipmentStatus { get; set; }
            public DateTime? ShippedAt { get; set; }
            public DateTime? DeliveredAt { get; set; }
            public string TrackingNo { get; set; }
        }


    }

