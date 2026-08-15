using MediatR;
using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NandiniSareesAPIs.Features.ProductImages.Commands
{
    public class SetPrimaryImageHandler : IRequestHandler<SetPrimaryImageCommand, bool>
    {
        private readonly IWriteDbContext _writeDb;
        private readonly IReadDbContext _readDb;

        public SetPrimaryImageHandler(IWriteDbContext writeDb, IReadDbContext readDb)
        {
            _writeDb = writeDb;
            _readDb = readDb;
        }

        public async Task<bool> Handle(SetPrimaryImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _readDb.ProductImages.FirstOrDefaultAsync(pi => pi.Id == request.ImageId, cancellationToken);
            if (image == null) return false;

            // unset others
            var others = _writeDb.Set<ProductImage>().Where(pi => pi.ProductId == image.ProductId && pi.Id != image.Id);
            await others.ForEachAsync(pi => pi.IsPrimary = false, cancellationToken);

            // set this
            var set = _writeDb.Set<ProductImage>();
            var tracked = set.Local.FirstOrDefault(pi => pi.Id == image.Id) ?? image;
            tracked.IsPrimary = true;

            await _writeDb.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
