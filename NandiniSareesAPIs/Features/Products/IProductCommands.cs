using System.Threading.Tasks;

namespace NandiniSareesAPIs.Features.Products
{
    public interface IProductCommands
    {
        Task<int> CreateAsync(CreateProductRequest request);
        Task<bool> UpdateAsync(int id, UpdateProductRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
