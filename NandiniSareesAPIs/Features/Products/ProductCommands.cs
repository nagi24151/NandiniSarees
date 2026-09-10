using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;

namespace NandiniSareesAPIs.Features.Products
{
    public class ProductCommands : IProductCommands
    {
        private readonly IWriteDbContext _write;

        public ProductCommands(IWriteDbContext write)
        {
            _write = write;
        }

        public async Task<int> CreateAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                SKU = request.SKU,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                //FinalPrice = request.FinalPrice,
                Discount = request.Discount,
                Stock = request.Stock,
                IsActive = request.IsActive
            };

            _write.Set<Product>().Add(product);
            await _write.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductRequest request)
        {
            var set = _write.Set<Product>();
            var product = await set.FindAsync(id);
            if (product == null) return false;

            if (request.Name != null) product.Name = request.Name;
            if (request.SKU != null) product.SKU = request.SKU;
            if (request.Description != null) product.Description = request.Description;
            if (request.CategoryId.HasValue) product.CategoryId = request.CategoryId;
            if (request.Price.HasValue) product.Price = request.Price.Value;
            //if (request.FinalPrice.HasValue) product.FinalPrice = request.FinalPrice.Value;
            if (request.Discount.HasValue) product.Discount = request.Discount.Value;
            if (request.Stock.HasValue) product.Stock = request.Stock.Value;
            if (request.IsActive.HasValue) product.IsActive = request.IsActive.Value;

            // set UpdatedAt if exists
            try { product.UpdatedAt = System.DateTime.UtcNow; } catch { }

            set.Update(product);
            await _write.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var set = _write.Set<Product>();
            var product = await set.FindAsync(id);
            if (product == null) return false;
            set.Remove(product);
            await _write.SaveChangesAsync();
            return true;
        }
    }
}
