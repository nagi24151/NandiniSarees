using Microsoft.AspNetCore.Http;

namespace NandiniSareesAPIs.Features.ProductImages.Requests
{
    public class UploadProductImageRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
