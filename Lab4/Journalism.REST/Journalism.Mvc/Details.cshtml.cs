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
    public class DetailsModel : PageModel
    {
        private readonly Journalism.Mvc.Data.ApplicationDbContext _context;

        public DetailsModel(Journalism.Mvc.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
