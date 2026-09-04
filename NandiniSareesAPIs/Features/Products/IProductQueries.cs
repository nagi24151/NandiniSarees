using System.Collections.Generic;
using System.Threading.Tasks;

namespace NandiniSareesAPIs.Features.Products
{
    public interface IProductQueries
    {
        Task<IEnumerable<ProductDto>> GetAllAsync(int? categoryId = null);

        Task<IEnumerable<ProductWithImageDto>> GetAllProductionsAsync();
        Task<ProductDto?> GetByIdAsync(int id);
    }
}
