using Microsoft.AspNetCore.Mvc;
using Journalism.RestApi.Models;
using Journalism.RestApi.Services;

namespace Journalism.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ICrudServiceAsync<ArticleModel> _service;

        public ArticlesController(ICrudServiceAsync<ArticleModel> service)
        {
            _service = service;
        }

        // GET: api/articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleModel>>> GetAll()
        {
            var items = await _service.ReadAllAsync();
            return Ok(items);
        }

        // GET: api/articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleModel>> GetById(int id)
        {
            var item = await _service.ReadAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/articles
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ArticleModel model)
        {
            await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/articles/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ArticleModel model)
        {
            if (id != model.Id) return BadRequest("Id в URL і тілі не збігаються");

            var ok = await _service.UpdateAsync(model);
            if (!ok) return NotFound();

            return NoContent();
        }

        // DELETE: api/articles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            await _service.RemoveAsync(existing);
            return NoContent();
        }
    }
}
