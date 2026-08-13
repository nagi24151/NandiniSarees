using System;
using System.Collections.Generic;

namespace NandiniSareesAPIs.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? PaymentId { get; set; }

        public User? User { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
        public Payment? Payment { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
