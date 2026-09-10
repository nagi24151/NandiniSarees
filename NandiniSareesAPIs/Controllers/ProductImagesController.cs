using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NandiniSareesAPIs.Features.ProductImages.Commands;
using NandiniSareesAPIs.Features.ProductImages.Queries;

namespace NandiniSareesAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId}/upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(int productId, [FromForm] Features.ProductImages.Requests.UploadProductImageRequest request, CancellationToken cancellationToken)
        {
            if (request?.File == null || request.File.Length == 0)
                return BadRequest("File is required.");

            var command = new UploadProductImageCommand(productId, request.File);
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(new { Id = id });
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetForProduct(int productId, CancellationToken cancellationToken)
        {
            var query = new GetProductImagesQuery(productId);
            var list = await _mediator.Send(query, cancellationToken);
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductImageCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/set-primary")]
        public async Task<IActionResult> SetPrimary(int id, CancellationToken cancellationToken)
        {
            var command = new SetPrimaryImageCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
