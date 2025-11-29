using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Journalism.Mvc.Data;
using Journalism.Mvc.Models;

namespace Journalism.Mvc
{
    public class EditModel : PageModel
    {
        private readonly Journalism.Mvc.Data.ApplicationDbContext _context;

        public EditModel(Journalism.Mvc.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public JournalistModel JournalistModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Journalists == null)
            {
                return NotFound();
            }

            var journalistmodel =  await _context.Journalists.FirstOrDefaultAsync(m => m.Id == id);
            if (journalistmodel == null)
            {
                return NotFound();
            }
            JournalistModel = journalistmodel;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(JournalistModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalistModelExists(JournalistModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool JournalistModelExists(int id)
        {
          return (_context.Journalists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
