using Journalism.Infrastructure.Models;
using Journalism.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Journalism.REST.Models;

namespace Journalism.REST.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ArticlesController : ControllerBase
	{
		private readonly ICrudServiceAsync<ArticleModel> _service;

		public ArticlesController(ICrudServiceAsync<ArticleModel> service)
		{
			_service = service;
		}

		

		// GET: api/Articles
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<ArticleModel>>> GetAll()
		{
			var items = await _service.ReadAllAsync();
			return Ok(items);
		}

		// GET: api/Articles/5
		[HttpGet("{id:int}")]
		[AllowAnonymous]
		public async Task<ActionResult<ArticleModel>> GetById(int id)
		{
			var item = await _service.ReadAsync(id);
			if (item == null) return NotFound();

			return Ok(item);
		}



		[HttpPost]
		[Authorize(Roles = "Journalist,Editor")]
		public async Task<ActionResult<ArticleModel>> Create([FromBody] ArticleCreateRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var article = new ArticleModel
			{
				Title = request.Title,
				Category = request.Category,
				JournalistId = request.JournalistId
			};

			await _service.CreateAsync(article);

			return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
		}


		// PUT: api/Articles/5
		[HttpPut("{id:int}")]
		[Authorize(Roles = "Journalist,Editor")]
		public async Task<ActionResult> Update(int id, [FromBody] ArticleModel article)
		{
			if (id != article.Id) return BadRequest();

			var ok = await _service.UpdateAsync(article);
			if (!ok) return NotFound();

			return NoContent();
		}

		// DELETE: api/Articles/5
		[HttpDelete("{id:int}")]
		[Authorize(Roles = "Editor")] // тільки редактор може видаляти
		public async Task<ActionResult> Delete(int id)
		{
			var dummy = new ArticleModel { Id = id };
			var ok = await _service.RemoveAsync(dummy);
			if (!ok) return NotFound();

			return NoContent();
		}
	}
}
