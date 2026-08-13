using System.Collections.Generic;

namespace NandiniSareesAPIs.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }
}
