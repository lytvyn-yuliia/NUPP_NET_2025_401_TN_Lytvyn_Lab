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
    public class JournalistModelsController : Controller
    {
        private readonly JournalismContext _context;

        public JournalistModelsController(JournalismContext context)
        {
            _context = context;
        }

        // GET: JournalistModels
        public async Task<IActionResult> Index()
        {
              return _context.Journalists != null ? 
                          View(await _context.Journalists.ToListAsync()) :
                          Problem("Entity set 'JournalismContext.Journalists'  is null.");
        }

        // GET: JournalistModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Journalists == null)
            {
                return NotFound();
            }

            var journalistModel = await _context.Journalists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (journalistModel == null)
            {
                return NotFound();
            }

            return View(journalistModel);
        }

        // GET: JournalistModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JournalistModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Experience,City")] JournalistModel journalistModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(journalistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(journalistModel);
        }

        // GET: JournalistModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Journalists == null)
            {
                return NotFound();
            }

            var journalistModel = await _context.Journalists.FindAsync(id);
            if (journalistModel == null)
            {
                return NotFound();
            }
            return View(journalistModel);
        }

        // POST: JournalistModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Experience,City")] JournalistModel journalistModel)
        {
            if (id != journalistModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(journalistModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JournalistModelExists(journalistModel.Id))
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
            return View(journalistModel);
        }

        // GET: JournalistModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Journalists == null)
            {
                return NotFound();
            }

            var journalistModel = await _context.Journalists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (journalistModel == null)
            {
                return NotFound();
            }

            return View(journalistModel);
        }

        // POST: JournalistModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Journalists == null)
            {
                return Problem("Entity set 'JournalismContext.Journalists'  is null.");
            }
            var journalistModel = await _context.Journalists.FindAsync(id);
            if (journalistModel != null)
            {
                _context.Journalists.Remove(journalistModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JournalistModelExists(int id)
        {
          return (_context.Journalists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
