using Journalism.REST.Models;
using Journalism.REST.Services;
using Journalism.RestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Journalism.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JournalistsController : ControllerBase
    {
        private readonly ICrudServiceAsync<JournalistModel> _service;

        public JournalistsController(ICrudServiceAsync<JournalistModel> service)
        {
            _service = service;
        }

        // GET: api/Journalists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalistModel>>> GetAll()
        {
            var items = await _service.ReadAllAsync();
            return Ok(items);
        }

        // GET: api/Journalists/page/1/10
        [HttpGet("page/{page:int}/{amount:int}")]
        public async Task<ActionResult<IEnumerable<JournalistModel>>> GetPage(int page, int amount)
        {
            var items = await _service.ReadAllAsync(page, amount);
            return Ok(items);
        }

        // GET: api/Journalists/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<JournalistModel>> GetById(int id)
        {
            var item = await _service.ReadAsync(id);
            if (item is null) return NotFound();

            return Ok(item);
        }

        // POST: api/Journalists
        [HttpPost]
        public async Task<ActionResult<JournalistModel>> Create([FromBody] JournalistModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _service.CreateAsync(model);
            if (!ok) return BadRequest("Не вдалося створити журналіста.");

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/Journalists/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] JournalistModel model)
        {
            if (id != model.Id)
                return BadRequest("Id в маршруті та тілі запиту мають збігатися.");

            var exists = await _service.ReadAsync(id);
            if (exists is null) return NotFound();

            var ok = await _service.UpdateAsync(model);
            if (!ok) return StatusCode(500, "Помилка при оновленні журналіста.");

            return NoContent(); // 204
        }

        // DELETE: api/Journalists/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.ReadAsync(id);
            if (existing is null) return NotFound();

            var ok = await _service.RemoveAsync(existing);
            if (!ok) return StatusCode(500, "Помилка при видаленні журналіста.");

            return NoContent();
        }
    }
}
