using MediatR;

namespace NandiniSareesAPIs.Features.ProductImages.Commands
{
    public record DeleteProductImageCommand(int Id) : IRequest<bool>;
}
