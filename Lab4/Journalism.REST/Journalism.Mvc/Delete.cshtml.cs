using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Journalism.Mvc.Data;
using Journalism.Mvc.Models;

namespace Journalism.Mvc
{
    public class DeleteModel : PageModel
    {
        private readonly Journalism.Mvc.Data.ApplicationDbContext _context;

        public DeleteModel(Journalism.Mvc.Data.ApplicationDbContext context)
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

            var journalistmodel = await _context.Journalists.FirstOrDefaultAsync(m => m.Id == id);

            if (journalistmodel == null)
            {
                return NotFound();
            }
            else 
            {
                JournalistModel = journalistmodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Journalists == null)
            {
                return NotFound();
            }
            var journalistmodel = await _context.Journalists.FindAsync(id);

            if (journalistmodel != null)
            {
                JournalistModel = journalistmodel;
                _context.Journalists.Remove(JournalistModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
