using MediatR;
using System.Collections.Generic;
using NandiniSareesAPIs.Features.ProductImages;

namespace NandiniSareesAPIs.Features.ProductImages.Queries
{
    public record GetProductImagesQuery(int ProductId) : IRequest<List<ProductImageDto>>;
}
