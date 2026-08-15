using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NandiniSareesAPIs.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NandiniSareesAPIs.Features.ProductImages.Commands
{
    public class UploadProductImageHandler : IRequestHandler<UploadProductImageCommand, int>
    {
        private readonly IWriteDbContext _writeDb;
        private readonly IWebHostEnvironment _env;

        public UploadProductImageHandler(IWriteDbContext writeDb, IWebHostEnvironment env)
        {
            _writeDb = writeDb;
            _env = env;
        }

        public async Task<int> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
                throw new ArgumentException("File is required", nameof(request.File));

            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "products");
            Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(fs, cancellationToken);
            }

            var url = $"/images/products/{fileName}";

            var image = new ProductImage
            {
                ProductId = request.ProductId,
                Url = url,
                AltText = null,
                SortOrder = 0,
                IsPrimary = false
            };

            _ = _writeDb.Set<ProductImage>().Add(image);
            await _writeDb.SaveChangesAsync(cancellationToken);

            return image.Id;
        }
    }
}
