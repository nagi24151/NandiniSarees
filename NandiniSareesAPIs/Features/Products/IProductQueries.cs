using System.Collections.Generic;
using System.Threading.Tasks;

namespace NandiniSareesAPIs.Features.Products
{
    public interface IProductQueries
    {
        Task<IEnumerable<ProductWithImageDto>> GetAllAsync(int? categoryId = null);

        Task<IEnumerable<ProductWithImageDto>> GetAllProductionsAsync();
        Task<ProductWithImageDto?> GetByIdAsync(int id);
    }
}
