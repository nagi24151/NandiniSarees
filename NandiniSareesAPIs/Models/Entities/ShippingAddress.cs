namespace NandiniSareesAPIs.Models
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = null!;
        public string? Phone { get; set; }

        public User? User { get; set; }
    }
}
