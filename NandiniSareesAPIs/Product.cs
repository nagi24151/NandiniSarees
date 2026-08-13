using System.ComponentModel.DataAnnotations;

namespace NandiniSareesAPIs
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
