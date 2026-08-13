using System;

namespace NandiniSareesAPIs.Features.Products
{
    public record ProductDto(
        int Id,
        string Name,
        string? SKU,
        string? Description,
        int? CategoryId,
        decimal Price,
        decimal? DiscountPrice,
        int Stock,
        bool IsActive
    );

    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string? SKU { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public string? SKU { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? Stock { get; set; }
        public bool? IsActive { get; set; }
    }
}
