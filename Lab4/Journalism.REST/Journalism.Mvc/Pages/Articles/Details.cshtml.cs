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
    public class DetailsModel : PageModel
    {
        private readonly Journalism.Mvc.Data.ApplicationDbContext _context;

        public DetailsModel(Journalism.Mvc.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
