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
    public class IndexModel : PageModel
    {
        private readonly Journalism.Mvc.Data.ApplicationDbContext _context;

        public IndexModel(Journalism.Mvc.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<JournalistModel> JournalistModel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Journalists != null)
            {
                JournalistModel = await _context.Journalists.ToListAsync();
            }
        }
    }
}
