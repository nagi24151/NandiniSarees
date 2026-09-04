namespace NandiniSareesAPIs.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; } = null!;
        public string? AltText { get; set; }
        public int SortOrder { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsItVideo { get; set; }

        public Product Product { get; set; } = null!;
    }
}
