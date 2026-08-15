using MediatR;
using Microsoft.AspNetCore.Hosting;
using NandiniSareesAPIs.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NandiniSareesAPIs.Features.ProductImages.Commands
{
    public class DeleteProductImageHandler : IRequestHandler<DeleteProductImageCommand, bool>
    {
        private readonly IWriteDbContext _writeDb;
        private readonly IReadDbContext _readDb;
        private readonly IWebHostEnvironment _env;

        public DeleteProductImageHandler(IWriteDbContext writeDb, IReadDbContext readDb, IWebHostEnvironment env)
        {
            _writeDb = writeDb;
            _readDb = readDb;
            _env = env;
        }

        public async Task<bool> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _readDb.ProductImages.FirstOrDefaultAsync(pi => pi.Id == request.Id, cancellationToken);
            if (image == null) return false;

            // Attempt to delete file on disk if it's a local URL
            try
            {
                if (!string.IsNullOrEmpty(image.Url) && image.Url.StartsWith("/"))
                {
                    var filePath = Path.Combine(_env.WebRootPath ?? "wwwroot", image.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(filePath)) File.Delete(filePath);
                }
            }
            catch
            {
                // ignore file deletion errors
            }

            var set = _writeDb.Set<ProductImage>();
            set.Remove(image);
            await _writeDb.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
