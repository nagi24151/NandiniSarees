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
        private readonly Microsoft.Extensions.Logging.ILogger<GetProductImagesHandler> _logger;

        public GetProductImagesHandler(IReadDbContext db, Microsoft.Extensions.Logging.ILogger<GetProductImagesHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<ProductImageDto>> Handle(GetProductImagesQuery request, CancellationToken cancellationToken)
        {
            try
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
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get product images for ProductId={ProductId}", request.ProductId);
                // Return empty list instead of throwing to avoid 500 in environments where DB is unreachable.
                return new List<ProductImageDto>();
            }
        }
    }
}
