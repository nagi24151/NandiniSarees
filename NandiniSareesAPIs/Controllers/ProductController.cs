using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NandiniSareesAPIs.Features.Products;

namespace NandiniSareesAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductQueries _queries;
        private readonly IProductCommands _commands;

        public ProductController(IProductQueries queries, IProductCommands commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
        {
            var items = await _queries.GetAllAsync(categoryId);
            return Ok(items);
        }

        [HttpGet("GetAllProductions")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _queries.GetAllProductionsAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _queries.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _commands.CreateAsync(request);
            var created = await _queries.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            var ok = await _commands.UpdateAsync(id, request);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _commands.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
