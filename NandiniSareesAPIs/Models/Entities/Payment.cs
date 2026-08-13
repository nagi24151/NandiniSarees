using System;

namespace NandiniSareesAPIs.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? Status { get; set; }

        public Order? Order { get; set; }
        public User? User { get; set; }
    }
}
