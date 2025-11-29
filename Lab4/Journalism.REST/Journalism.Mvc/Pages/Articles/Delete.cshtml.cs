using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Journalism.Mvc.Data;
using Journalism.Mvc.Models;

namespace Journalism.Mvc.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly Journalism.Mvc.Data.ApplicationDbContext _context;

        public DeleteModel(Journalism.Mvc.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public ArticleModel ArticleModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var articlemodel = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);

            if (articlemodel == null)
            {
                return NotFound();
            }
            else 
            {
                ArticleModel = articlemodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }
            var articlemodel = await _context.Articles.FindAsync(id);

            if (articlemodel != null)
            {
                ArticleModel = articlemodel;
                _context.Articles.Remove(ArticleModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
