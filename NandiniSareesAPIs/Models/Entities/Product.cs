using System;
using System.Collections.Generic;

namespace NandiniSareesAPIs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? SKU { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal? Discount { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Category? Category { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
