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
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public UploadProductImageHandler(IWriteDbContext writeDb, IWebHostEnvironment env, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _writeDb = writeDb;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
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

            // Determine base URL: prefer configured App:BaseUrl, otherwise derive from the current HTTP request if available.
            //var configuredBase = _configuration["App:BaseUrl"];
            string baseUrl = string.Empty;
            //if (!string.IsNullOrWhiteSpace(configuredBase))
            //{
            //    baseUrl = configuredBase.TrimEnd('/');
            //}
            //else 
            if (_httpContextAccessor?.HttpContext != null)
            {
                var req = _httpContextAccessor.HttpContext.Request;
                baseUrl = $"{req.Scheme}://{req.Host.Value}".TrimEnd('/');
            }

            var url = string.IsNullOrEmpty(baseUrl)
                ? $"/images/products/{fileName}"
                : $"{baseUrl}/images/products/{fileName}";

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
