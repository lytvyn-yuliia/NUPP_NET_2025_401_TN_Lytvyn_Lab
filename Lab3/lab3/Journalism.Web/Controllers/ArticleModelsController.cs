using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Journalism.Infrastructure;
using Journalism.Infrastructure.Models;

namespace Journalism.Web.Controllers
{
    public class ArticleModelsController : Controller
    {
        private readonly JournalismContext _context;

        public ArticleModelsController(JournalismContext context)
        {
            _context = context;
        }

        // GET: ArticleModels
        public async Task<IActionResult> Index()
        {
            var journalismContext = _context.Articles.Include(a => a.Journalist);
            return View(await journalismContext.ToListAsync());
        }

        // GET: ArticleModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles
                .Include(a => a.Journalist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // GET: ArticleModels/Create
        public IActionResult Create()
        {
            ViewData["JournalistId"] = new SelectList(_context.Journalists, "Id", "City");
            return View();
        }

        // POST: ArticleModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Category,JournalistId")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JournalistId"] = new SelectList(_context.Journalists, "Id", "City", articleModel.JournalistId);
            return View(articleModel);
        }

        // GET: ArticleModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.FindAsync(id);
            if (articleModel == null)
            {
                return NotFound();
            }
            ViewData["JournalistId"] = new SelectList(_context.Journalists, "Id", "City", articleModel.JournalistId);
            return View(articleModel);
        }

        // POST: ArticleModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,JournalistId")] ArticleModel articleModel)
        {
            if (id != articleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleModelExists(articleModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["JournalistId"] = new SelectList(_context.Journalists, "Id", "City", articleModel.JournalistId);
            return View(articleModel);
        }

        // GET: ArticleModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles
                .Include(a => a.Journalist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // POST: ArticleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Articles == null)
            {
                return Problem("Entity set 'JournalismContext.Articles'  is null.");
            }
            var articleModel = await _context.Articles.FindAsync(id);
            if (articleModel != null)
            {
                _context.Articles.Remove(articleModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleModelExists(int id)
        {
          return (_context.Articles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
