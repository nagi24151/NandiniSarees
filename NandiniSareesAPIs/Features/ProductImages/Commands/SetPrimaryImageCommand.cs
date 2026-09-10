using MediatR;

namespace NandiniSareesAPIs.Features.ProductImages.Commands
{
    public record SetPrimaryImageCommand(int ImageId) : IRequest<bool>;
}
