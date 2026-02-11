using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.GlobalCall
{
    public static class ShipmentStatusOrder
    {
        public static readonly IReadOnlyDictionary<string, int> Map
            = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                ["READY_TO_SHIP"] = 1,
                ["PICKUP_SCHEDULED"] = 2,
                ["PICKED_UP"] = 3,
                ["IN_TRANSIT"] = 4,
                ["OUT_FOR_DELIVERY"] = 5,
                ["DELIVERED"] = 6,
                ["FAILED"] = 7,
                ["RETURN_TO_ORIGIN"] = 8
            };

        public static int GetOrder(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return 0;

            return Map.TryGetValue(status, out var order)
                ? order
                : 0;
        }
    }
}
