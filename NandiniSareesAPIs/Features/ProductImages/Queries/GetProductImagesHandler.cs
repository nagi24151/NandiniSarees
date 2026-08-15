using MediatR;
using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NandiniSareesAPIs.Features.ProductImages.Queries
{
    public class GetProductImagesHandler : IRequestHandler<GetProductImagesQuery, List<ProductImageDto>>
    {
        private readonly IReadDbContext _db;

        public GetProductImagesHandler(IReadDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProductImageDto>> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
        {
            return await _db.ProductImages
                .Where(pi => pi.ProductId == request.ProductId)
                .OrderBy(pi => pi.SortOrder)
                .Select(pi => new ProductImageDto
                {
                    Id = pi.Id,
                    ProductId = pi.ProductId,
                    Url = pi.Url,
                    AltText = pi.AltText,
                    SortOrder = pi.SortOrder,
                    IsPrimary = pi.IsPrimary
                })
                .ToListAsync(cancellationToken);
        }
    }
}
