using System;
using System.Collections.Generic;

namespace NandiniSareesAPIs.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User? User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
