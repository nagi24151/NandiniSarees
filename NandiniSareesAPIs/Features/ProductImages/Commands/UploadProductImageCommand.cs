using MediatR;
using Microsoft.AspNetCore.Http;

namespace NandiniSareesAPIs.Features.ProductImages.Commands
{
    public record UploadProductImageCommand(int ProductId, IFormFile File, string? AltText = null, int SortOrder = 0, bool IsPrimary = false) : IRequest<int>;
}
