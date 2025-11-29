using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Journalism.Mvc.Data;
using Journalism.Mvc.Models;

namespace Journalism.Mvc
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
        public JournalistModel JournalistModel { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Journalists == null || JournalistModel == null)
            {
                return Page();
            }

            _context.Journalists.Add(JournalistModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
