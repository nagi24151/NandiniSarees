using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;

namespace NandiniSareesAPIs.Features.Products
{
    public class ProductQueries : IProductQueries
    {
        private readonly IReadDbContext _read;

        public ProductQueries(IReadDbContext read)
        {
            _read = read;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(int? categoryId = null)
        {
            var q = _read.Products.AsNoTracking().AsQueryable();
            if (categoryId.HasValue)
                q = q.Where(p => p.CategoryId == categoryId.Value);

            var list = await q.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.SKU,
                p.Description,
                p.CategoryId,
                p.Price,
                p.FinalPrice,
                p.Discount,
                p.Stock,
                p.IsActive
            )).ToListAsync();

            return list;
        }

        public async Task<IEnumerable<ProductWithImageDto>> GetAllProductionsAsync()
        {
            // Return products with a representative image URL (primary image if available, otherwise first image)
            var list = await _read.Products
                .AsNoTracking()
                .Select(p => new ProductWithImageDto(
                    p.Id,
                    p.Name,
                    p.SKU,
                    p.Description,
                    p.CategoryId,
                    p.Price,
                    p.FinalPrice,
                    p.Discount,
                    p.Stock,
                    p.IsActive,
                    p.Images.OrderByDescending(i => i.IsPrimary)
                            .ThenBy(i => i.SortOrder)
                            .Select(i => i)
                            .ToList()
                ))
                .ToListAsync();

            return list;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _read.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return null;
            return new ProductDto(p.Id, p.Name, p.SKU, p.Description, p.CategoryId, p.Price, p.FinalPrice, p.Discount, p.Stock, p.IsActive);
        }
    }
}
