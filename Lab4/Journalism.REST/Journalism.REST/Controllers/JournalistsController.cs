using Journalism.Infrastructure.Models;
using Journalism.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journalism.REST.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize] 
	public class JournalistsController : ControllerBase
	{
		private readonly ICrudServiceAsync<JournalistModel> _service;

		public JournalistsController(ICrudServiceAsync<JournalistModel> service)
		{
			_service = service;
		}

		// GET: api/Journalists
		[HttpGet]
		[Authorize(Roles = "Journalist,Editor")]
		public async Task<ActionResult<IEnumerable<JournalistModel>>> GetAll()
		{
			var items = await _service.ReadAllAsync();
			return Ok(items);
		}

		// GET: api/Journalists/5
		[HttpGet("{id:int}")]
		[Authorize(Roles = "Journalist,Editor")]
		public async Task<ActionResult<JournalistModel>> GetById(int id)
		{
			var item = await _service.ReadAsync(id);
			if (item == null) return NotFound();

			return Ok(item);
		}

		

		[HttpPost]
		[Authorize(Roles = "Editor")]
		public async Task<ActionResult> Create([FromBody] JournalistModel journalist)
		{
			var ok = await _service.CreateAsync(journalist);
			if (!ok) return BadRequest();

			return CreatedAtAction(nameof(GetById), new { id = journalist.Id }, journalist);
		}

		[HttpPut("{id:int}")]
		[Authorize(Roles = "Editor")]
		public async Task<ActionResult> Update(int id, [FromBody] JournalistModel journalist)
		{
			if (id != journalist.Id) return BadRequest();

			var ok = await _service.UpdateAsync(journalist);
			if (!ok) return NotFound();

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		[Authorize(Roles = "Editor")]
		public async Task<ActionResult> Delete(int id)
		{
			var dummy = new JournalistModel { Id = id };
			var ok = await _service.RemoveAsync(dummy);
			if (!ok) return NotFound();

			return NoContent();
		}
	}
}
