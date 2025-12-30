namespace UrbanNative.Application.DTOs.Address
{
    public class AddressCreateDto
    {
        public string EntityType { get; set; } = default!;
        public int EntityID { get; set; }

        public string AddressLine1 { get; set; } = default!;
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }

        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }

        public string Pincode { get; set; } = default!;

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public string AddressType { get; set; } = default!; // Billing | Shipping
        public bool IsPrimary { get; set; }
    }

    public class AddressDto
    {
        public int AddressID { get; set; }

        public string AddressLine1 { get; set; } = default!;
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }

        public string CountryName { get; set; } = default!;
        public string StateName { get; set; } = default!;
        public string CityName { get; set; } = default!;

        public string Pincode { get; set; } = default!;
        public string AddressType { get; set; } = default!;
        public bool IsPrimary { get; set; }
    }
}