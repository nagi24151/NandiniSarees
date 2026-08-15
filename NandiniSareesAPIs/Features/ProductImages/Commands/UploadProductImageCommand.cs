using MediatR;
using Microsoft.AspNetCore.Http;

namespace NandiniSareesAPIs.Features.ProductImages.Commands
{
    public record UploadProductImageCommand(int ProductId, IFormFile File) : IRequest<int>;
}
