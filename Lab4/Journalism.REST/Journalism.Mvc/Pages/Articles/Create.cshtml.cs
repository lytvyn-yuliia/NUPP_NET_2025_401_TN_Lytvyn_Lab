using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Journalism.Mvc.Data;
using Journalism.Mvc.Models;

namespace Journalism.Mvc.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Journalism.Mvc.Data.ApplicationDbContext _context;

        public CreateModel(Journalism.Mvc.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ArticleModel ArticleModel { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Articles == null || ArticleModel == null)
            {
                return Page();
            }

            _context.Articles.Add(ArticleModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
