using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NandiniSareesAPIs.Features.Users;

namespace NandiniSareesAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserQueries _queries;
        private readonly IUserCommands _commands;

        public UserController(IUserQueries queries, IUserCommands commands)
        {
            _queries = queries;
            _commands = commands;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _queries.GetAllAsync();
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
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _commands.CreateAsync(request);
            if (id <= 0) return Conflict(new { message = "Email already exists" });
            var created = await _queries.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
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
